using Microsoft.AspNetCore.Mvc;
using StarterPack.Models;
using StarterPack.Core.Auth;
using System.Linq;
using System;

namespace StarterPack.Core.Controllers
{
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Pega o usuário autenticado
        /// </summary>
        /// <returns>Usuário autenticado</returns>
        public User CurrentUser() {
            return AuthService.GetCurrentUser(HttpContext);
        }

        /// <summary>
        /// Pega um parametro da requisição
        /// </summary>
        /// <param name="name">Nome da chave</param>
        /// <returns>Parametro encontrado ou null</returns>
        public string GetParameter(string name) {
            return (HasParameter(name)) ? HttpContext.Request.Query[name].First() : null;
        }

        /// <summary>
        /// Pega um parametro da requisição e tipa para o tipo fornecidos.
        /// Tipos disponíveis: int, long, DateTime, string e boolean
        /// </summary>
        /// <param name="name">Nome da chave</param>
        /// <returns>Parametro encontrado ou null</returns>
        public T GetParameter<T>(string name) where T : IConvertible {
            if (HasParameter(name)) {
                var parameter = HttpContext.Request.Query[name].First();

                if(typeof(T) == typeof(int))
                    return (T) Convert.ChangeType(int.Parse(parameter), typeof(T));

                if(typeof(T) == typeof(long))
                    return (T) Convert.ChangeType(long.Parse(parameter), typeof(T));

                if(typeof(T) == typeof(DateTime))
                    return (T) Convert.ChangeType(DateTime.Parse(parameter), typeof(T));

                if(typeof(T) == typeof(bool))
                    return (T) Convert.ChangeType(bool.Parse(parameter), typeof(T));
            }

            return default(T);
        }

        /// <summary>
        /// Verifica se na requisição existe o parametro com a chave informada
        /// </summary>
        /// <param name="name">Nome da chave</param>
        /// <returns>True caso existe o parametro ou False caso não exista</returns>
        public bool HasParameter(string name) {
            return HttpContext.Request.Query.ContainsKey(name);
        }
    }
}
