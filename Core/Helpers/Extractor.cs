using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace StarterPack.Core.Helpers
{
    public class Extractor
    {
        /// <summary>
		/// Recupera o nome da ação (método) que está sendo chamado
		/// </summary>
		/// <param name="actionDescriptor"></param>
		/// <returns></returns>
        public static string GetActionName(ActionDescriptor actionDescriptor) {
			PropertyInfo[] properties = actionDescriptor.GetType().GetProperties();              

			if(properties != null) {					
				PropertyInfo prop = properties.First(p => p.Name == "ActionName");
				if(prop != null) {
					string actionName = (string)prop.GetValue(actionDescriptor);
					return actionName;
				}
			}
			return null;
		}

        public static List<string> GetRuleRoles(string actionName, string[] rules){
            string actionDelimiter = actionName.ToLower()+":";
			string rule = rules.First(r => r.ToLower().Contains(actionDelimiter));
			List<string> rolesAllowed = rule.Replace(actionDelimiter, "").Split(',').ToList();
            return rolesAllowed;
        }
    }
}