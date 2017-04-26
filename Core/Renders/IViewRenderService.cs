using System.Dynamic;
using System.Threading.Tasks;

namespace StarterPack.Core.Renders
{
    public interface IViewRenderService
    {
          Task<string> RenderToStringAsync(string viewName, ExpandoObject data);
    }
}