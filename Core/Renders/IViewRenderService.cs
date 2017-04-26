using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace StarterPack.Core.Renders
{
    public interface IViewRenderService
    {
          Task<string> RenderToStringAsync(string viewName, ViewDataDictionary model);
    }
}