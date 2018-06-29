using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace CompanyFinderAdmin.Infrastructure
{
    /// <summary>
    /// Render view service
    /// </summary>
    public class ViewRenderService : IViewRenderService
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// Constructor DI
        /// </summary>
        /// <param name="razorViewEngine"></param>
        /// <param name="tempDataProvider"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="httpContextAccessor"></param>
        public ViewRenderService(IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider,
            IHttpContextAccessor httpContextAccessor)
        {
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// Render view to string
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> RenderToStringAsync(string viewName, object model)
        {
            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using (var sw = new StringWriter())
            {
                //var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);
                var viewResult = _razorViewEngine.GetView("~/", viewName, false);

                if (viewResult == null)
                {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }

        //public string Render(string viewPath)
        //{
        //    return Render(viewPath, string.Empty);
        //}

        //public string Render<TModel>(string viewPath, TModel model)
        //{

        //    var viewEngineResult = _razorViewEngine.GetView("~/", viewPath, false);

        //    if (!viewEngineResult.Success)
        //    {
        //        throw new InvalidOperationException($"Couldn't find view {viewPath}");
        //    }

        //    var view = viewEngineResult.View;

        //    using (var output = new StringWriter())
        //    {
        //        var viewContext = new ViewContext();
        //        viewContext.HttpContext = _httpContextAccessor.HttpContext;
        //        viewContext.ViewData = new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
        //        { Model = model };
        //        viewContext.Writer = output;

        //        view.RenderAsync(viewContext).GetAwaiter().GetResult();

        //        return output.ToString();
        //    }
        //}
    }
}
