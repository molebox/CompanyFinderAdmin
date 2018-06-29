using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.Infrastructure
{
    /// <summary>
    /// Render View interface
    /// </summary>
    public interface IViewRenderService
    {
        /// <summary>
        /// RenderToStringAsync
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<string> RenderToStringAsync(string viewName, object model);
        //string Render<TModel>(string viewPath, TModel model);
    }
}
