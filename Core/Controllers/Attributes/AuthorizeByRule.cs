using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using StarterPack.Core.Helpers;
using StarterPack.Models;

namespace StarterPack.Core.Controllers.Attributes
{

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class AuthorizeByRule : AuthorizeAttribute, IActionFilter
	{	
		private string[] _rules;

		/// <summary>
		/// Defina, para cada action (método do controller) quais perfis (roles) são necessários, com o seguinte padrão: "[action-name]:role1,role2,role3".
		/// Se o usuário autenticado tiver pelo menos um dos perfis listados, a action poderá ser executada.
		/// Múltiplas rules (uma para cada action) podem ser especificadas. Ex.: AuthorizeByRule("index:admin", "store:manager, admin").
		/// </summary>
		/// <param name="rules"></param>
		public AuthorizeByRule(params string[] rules) {
			_rules = rules;
    }

		void IActionFilter.OnActionExecuted(ActionExecutedContext context)
		{
			// silence is gold
		}

		/// <summary>
		/// Verifica se o usuário corrente possui um dos perfil definidos para executar uma action de um controller.
		/// </summary>
		/// <param name="context"></param>
		void IActionFilter.OnActionExecuting(ActionExecutingContext context)
		{
			// Recupera o usuário corrente
			User user = Auth.Auth.CurrentUser(context.HttpContext.User);
			if(user != null){
				string actionName = Extractor.GetActionName(context.ActionDescriptor);
				
				// Extrai a lista de roles que têm permissão das regras definidas via anotação
				List<string> rolesAllowed = Extractor.GetRuleRoles(actionName, _rules);
			
				// Verifica se pelo menos um dos roles está associado ao perfil do usuário
				bool actionAllowed = false;
				foreach (string roleAllowed in rolesAllowed)
				{
					if(user.UserRoles.Any(ur => ur.Role.Slug.ToLower() == roleAllowed.ToLower())) {
						actionAllowed = true;
						break;
					}				
				}	
				// Se o usuário não tem pelo menos um dos roles necessário, nega a execução da action
				if(!actionAllowed){
					throw new NotImplementedException("method not available");
				}				
			} // se não há usuário autenticado, então a executação da ação não é permitida
			else {
				throw new NotImplementedException("method not available");
			}			
		}
	}
}

