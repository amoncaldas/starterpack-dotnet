using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using StarterPack.Core.Helpers;

namespace StarterPack.Core.Renders
{
    public class RazorRender : IViewRenderService
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
 
        public RazorRender(IRazorViewEngine razorViewEngine,ITempDataProvider tempDataProvider, IServiceProvider serviceProvider)
        {
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }
 
        public async Task<string> RenderToStringAsync(string viewName, ExpandoObject data)
        {
            var httpContext = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
            
 
            using (var sw = new StringWriter())
            {
                var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);
 
                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }
 
                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
                viewDictionary.Model = data;

                TempDataDictionary dict = new TempDataDictionary(actionContext.HttpContext, _tempDataProvider); 
                var viewContext = new ViewContext(actionContext, viewResult.View, viewDictionary, dict, sw, new HtmlHelperOptions());                
                PopulateViewData(viewContext.ViewData, data);           
 
                await viewResult.View.RenderAsync(viewContext);
                string view = sw.ToString();
                return view;
            }
        }

        public static IViewRenderService service {            
            get {
                return (IViewRenderService)Services.Resolve(typeof(IViewRenderService));
            }
        }

        protected void PopulateViewData(ViewDataDictionary viewData, ExpandoObject data){           
            IDictionary<string, object> propertyValues = (IDictionary<string, object>)data;
            foreach (var item in propertyValues)
            {               
                viewData[item.Key] = item.Value;
            }          
        }
    }
}