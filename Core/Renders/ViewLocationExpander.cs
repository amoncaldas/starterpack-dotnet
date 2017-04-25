using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Razor;

namespace StarterPack.Core.Renders
{
    public class ViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            return new[]
            {
                "/Resources/MailTemplates/{0}.cshtml"               
            };
        }
        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }
    }
}