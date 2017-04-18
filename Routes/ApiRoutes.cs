using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace StarterPack.Routes
{
	public static class ApiRoutes
	{
		public static IRouteBuilder get(IRouteBuilder routeBuilder){

			routeBuilder.MapRoute(
				name: "default_route",
    		template: "{controller}/{action}/{id?}"
			);

			return routeBuilder;
		}
	}
}