using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Mvc
{
	//
	// Summary:
	// Specifies that a parameter or property should be bound using the request query string. 
	///
	public class FromFilterAttribute : FromQueryAttribute, IParameterModelConvention, IActionConstraintMetadata
	{
			private IHttpContextAccessor _contextAccessor;
			private HttpContext _context { get { return _contextAccessor.HttpContext; } }
			public FromFilterAttribute(IHttpContextAccessor contextAccessor) {
				_contextAccessor = contextAccessor;
			}
			public void Apply(ParameterModel parameter)
			{
					if (parameter.Action.Selectors != null && parameter.Action.Selectors.Any())
					{
						
						var t = parameter;
							//parameter.Action.Selectors.Last().ActionConstraints.Add();
					}				
			}

			public bool Accept(ActionConstraintContext context) {
				return true;
			}
	}
}