using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace StarterPack.Core.Helpers
{
    public class Extractor
    {
        /// <summary>
		/// Recupera o nome da ação (método)(lowerCase) que está sendo chamado
		/// </summary>
		/// <param name="actionDescriptor"></param>
		/// <returns></returns>
        public static string GetActionName(ActionDescriptor actionDescriptor) {
			PropertyInfo[] properties = actionDescriptor.GetType().GetProperties();              

			if(properties != null) {					
				PropertyInfo prop = properties.First(p => p.Name == "ActionName");
				if(prop != null) {
					string actionName = (string)prop.GetValue(actionDescriptor);
					return actionName.ToLower();
				}
			}
			return null;
		}

        public static List<string> GetActionRoles(string actionName, string[] rules){            
			string actionDelimiter = actionName.ToLower()+":";
			string rule = rules.First(r => r.ToLower().Contains(actionDelimiter));
			List<string> rolesAllowed = rule.Replace(actionDelimiter, "").Split(',').ToList();

			return rolesAllowed;
        }

		public static bool ActionHasRoles(string actionName, string[] rules){            
			string roles = rules.FirstOrDefault(r => {
				return r.ToLower().Contains( actionName + ":" );
			});

			return !string.IsNullOrEmpty(roles);
		}


        public static List<string> GetActions(string[] rules){
			return rules.Select(actionWithProfiles => {
				string[] splitedActions = actionWithProfiles.ToLower().Split(':');

				if(splitedActions.Length == 1)
					return actionWithProfiles;
				else
					return splitedActions.First();
			}).ToList();
        }					
    }
}