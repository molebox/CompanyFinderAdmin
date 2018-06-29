using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CompanyFinderAdmin
{
    /// <summary>
    /// Program class where the app begins
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method that runs on start up
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }
        /// <summary>
        /// Build app
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args) =>
             WebHost.CreateDefaultBuilder(args)
                 .UseStartup<Startup>().UseDefaultServiceProvider(options =>
                     options.ValidateScopes = false).UseKestrel().UseIISIntegration()
                 .Build();
    }
}
