using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CompanyFinderAdmin.Data;
using CompanyFinderAdmin.Models;
using CompanyFinderAdmin.Services;
using DatabaseLibrary.Models;
using CompanyFinderAdmin.Email;
using CompanyFinderAdmin.Infrastructure;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;

namespace CompanyFinderAdmin
{
    /// <summary>
    /// Start up class that initializes middlewere
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<CompanyDbContext>(options =>
           options.UseSqlServer(Configuration["Data:CompanyConnectionString:CompanyConnection"]));

            services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(Configuration["Data:IdentityConnectionString:IdentityConnection"]));

            services.AddSingleton(Configuration);

            services.AddTransient<ICompanyRepository, CompanyRepository>();

            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });

            services.AddMemoryCache();
            services.AddSession();
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }).AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, options => options.ResourcesPath = "Resources")
              .AddDataAnnotationsLocalization().AddSessionStateTempDataProvider();

            services.Configure<RequestLocalizationOptions>(
            opts =>
            {
                var supportedCultures = new[]
                {
                        new CultureInfo("en-GB"), // English
                        new CultureInfo("sv-SE"), //Swedish
                        new CultureInfo("nb-NO"), //Norwegian Bokmål
                        new CultureInfo("fi-FI"), //Finnish
                        new CultureInfo("da-DK"), //Danish
                        new CultureInfo("fr-FR"), //French
                        new CultureInfo("de-DE"), //German
                        new CultureInfo("es-ES"), //Spanish
                        

               };

                opts.DefaultRequestCulture = new RequestCulture(culture: "en-GB");
                // Formatting numbers, dates, etc.
                opts.SupportedCultures = supportedCultures;
                // UI strings that we have localized.
                opts.SupportedUICultures = supportedCultures;
            });

            // Add application services.
            //services.AddTransient<IEmailSender, EmailSender>();
            services.AddSingleton<IEmailConfiguration>(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            services.AddTransient<IEmailService, EmailService>();




        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            // Add the custom roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManger = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            // Roles in the application
            string[] roleNames = { "Admin, TemplateAccess" };

            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                // Create the roles and seed them to the db
                var roleExists = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create the super admin
            var powerUser = new IdentityUser
            {
                UserName = Configuration.GetSection("Data:AdminSettings")["SuperAdminName"],
            };



            string adminPassword = Configuration.GetSection("Data:AdminSettings")["SuperAdminPassword"];
            var _user = await UserManger.FindByNameAsync(Configuration.GetSection("Data:AdminSettings")["SuperAdminName"]);

            if (_user == null)
            {
                var createSuperUser = await UserManger.CreateAsync(powerUser, adminPassword);
                if (createSuperUser.Succeeded)
                {
                    // Give the new user admin role
                    await UserManger.AddToRoleAsync(powerUser, "Admin");
                }
            }

            //// Create the user
            var normalUser = new IdentityUser
            {
                UserName = Configuration.GetSection("Data:TemplateSettings")["UserName"],
            };

            string userPassword = Configuration.GetSection("Data:TemplateSettings")["UserPassword"];
            var _normalUser = await UserManger.FindByNameAsync(Configuration.GetSection("Data:TemplateSettings")["UserName"]);

            if (_normalUser == null)
            {
                var createUser = await UserManger.CreateAsync(normalUser, userPassword);
                if (createUser.Succeeded)
                {
                    // Give the new user a role
                    await UserManger.AddToRoleAsync(normalUser, "TemplateAccess");
                }
            }
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="serviceProvider"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            try
            {   //Create Identity Database on first start up if not already exists
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                    .CreateScope())
                {
                    serviceScope.ServiceProvider.GetService<AppIdentityDbContext>().Database.Migrate();

                }
            }
            catch (Exception ex)
            {

                Console.Write(ex);
            }

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
            app.UseSession();
            app.UseMvc(routes =>
            {

                //routes.MapRoute("CompTemplate", "companyTemplate/{id}",
                //new { controller = "Template", action = "CompanyTemplate", id = RandomUrl.UniqueUrl() });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Admin}/{action=Login}/{id?}");
            });

            CreateRoles(serviceProvider).Wait();
        }
    }
}
