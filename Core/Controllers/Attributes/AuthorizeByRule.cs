
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using StarterPack.Models;

namespace StarterPack.Core.Controllers.Attributes
{//
	// Summary:
	//     /// Specifies that the class or method that this attribute is applied to requires
	//     the specified authorization. ///
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class AuthorizeByRule : AuthorizeAttribute, IActionFilter
	{	
		public AuthorizeByRule() {
             
        }

		void IActionFilter.OnActionExecuted(ActionExecutedContext context)
		{
			// silence is gold
		}

		void IActionFilter.OnActionExecuting(ActionExecutingContext context)
		{
			
		}
	}
}

