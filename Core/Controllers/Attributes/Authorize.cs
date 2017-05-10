using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Filters;
using StarterPack.Core.Helpers;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Primitives;
using StarterPack.Core.Exception;
using System.Linq;
using StarterPack.Core.Auth;

namespace StarterPack.Core.Controllers.Attributes
{

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
	public class Authorize : Attribute, IActionFilter
	{	
		private string[] _rules;

		/// <summary>
		/// Defina, para cada action (método do controller) quais perfis (roles) são necessários, com o seguinte padrão: "[action-name]:role1,role2,role3".
		/// Se o usuário autenticado tiver pelo menos um dos perfis listados, a action poderá ser executada.
		/// Múltiplas rules (uma para cada action) podem ser especificadas. Ex.: AuthorizeByRule("index:admin", "store:manager, admin").
		/// </summary>
		/// <param name="rules">Ex.: AuthorizeByRule("index:admin", "store:manager, admin").</param>
		public Authorize(params string[] rules) {
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
			if( _rules == null ) {
				Authorize.ValidateToken(context);
			} else {
				string actionName = Extractor.GetActionName(context.ActionDescriptor);

				List<string> allActions = Extractor.GetActions(_rules);

				// Só requer autenticação se a action se tiver na relação de rules
				if(allActions.Contains(actionName) || allActions.Contains("*")) {
					Authorize.ValidateToken(context);

					// Valida os perfis setado para action coringa *
					Authorize.ValidateRole("*", _rules, context);

					// Só valida por perfil se a action tiver algum profile
					Authorize.ValidateRole(actionName, _rules, context);
				}			
			}
		}

		private static void ValidateRole(string actionName, string[] rules, ActionExecutingContext context) {
			if(Extractor.ActionHasRoles(actionName, rules)) {
				// Extrai a lista de roles que têm permissão das regras definidas via anotação
				List<string> rolesAllowed = Extractor.GetActionRoles(actionName, rules);
			
				// Verifica se pelo menos um dos roles está associado ao perfil do usuário
				bool actionAllowed = false;
				foreach (string roleAllowed in rolesAllowed)
				{	
					actionAllowed = AuthService.GetCurrentUser(context.HttpContext).HasRole(roleAllowed);
					break;
				}	

				// Se o usuário não tem pelo menos um dos roles necessário, nega a execução da action
				if(!actionAllowed){
					throw new ApiException("messages.notAuthorized", 401);
				}	
			}	
		}

		private static String GetToken(ActionExecutingContext context) {
			StringValues authorizationHeader;
			
			var hasAuthorization = context.HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationHeader);

			if(hasAuthorization) {
				return authorizationHeader.First().Replace("Bearer ", "");
			}

			return null;
		}

		private static void ValidateToken(ActionExecutingContext context) {
			TokenValidationParameters parameters = Services.Resolve<TokenValidationParameters>();
			SecurityToken validatedToken;			

			//Faz essa verificação para não precisar validar o token novamente caso a anotações seja utilizada
			//mais de 1 vez.
			if( AuthService.GetCurrentUser(context.HttpContext) == null && AuthService.GetToken(context.HttpContext) == null ) {
				var token = Authorize.GetToken(context);

				if(token != null) {
					try {
						context.HttpContext.User = new JwtSecurityTokenHandler().ValidateToken(Authorize.GetToken(context), parameters, out validatedToken);
						AuthService.SetCurrentUser(context.HttpContext, validatedToken);
					} catch(ArgumentException) { //formato invalido
						throw new ApiException("token_invalid", 401);	
					} catch(SecurityTokenExpiredException) { //tempo expirado
						throw new ApiException("token_expired", 401);	
					} catch(SecurityTokenException)	{ //qualquer outra validação
						throw new ApiException("token_invalid", 401);
					}	
				} else {
					throw new ApiException("token_not_provided", 401);
				}	
			}							
		}		
	}
}

