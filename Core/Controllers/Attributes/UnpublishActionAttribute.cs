using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StarterPack.Core.Helpers;

namespace StarterPack.Core.Controllers.Attributes
{   
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public class UnpublishActionAttribute: Attribute, IBindingSourceMetadata, IActionFilter
	{
		private string[] _unpublishMethods;
		
		/// <summary>
		/// Informe o nome da(s) ação(ões) [métodos] existentes no CrudController (Index, Get, Update, Destroy) que deve(m) ser tornadas não acessíveis neste controller
		/// </summary>
		/// <param name="unpublishMethods"></param>
		public UnpublishActionAttribute(params string[] unpublishMethods) { 
			_unpublishMethods = unpublishMethods;			
    }

		public BindingSource BindingSource { get; }

		void IActionFilter.OnActionExecuted(ActionExecutedContext context)
		{
				// silence is gold
		}

		/// <summary>
		/// Este método é executado antes da ação do controller ser executada. Aqui verificamos que a ação foi desativada via anotação 
		/// </summary>
		/// <param name="context"></param>
		void IActionFilter.OnActionExecuting(ActionExecutingContext context)
		{
			string actionName = Extractor.GetActionName(context.ActionDescriptor);		
			if(actionName != null && _unpublishMethods.Any(m => m.ToString().ToLower() == actionName.ToLower())){
				throw new NotImplementedException("method not available");
			}
		}
	}
}
