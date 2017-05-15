using Microsoft.AspNetCore.Mvc;
using StarterPack.Models;
using StarterPack.Core.Auth;
using System.Linq;
using System;

namespace StarterPack.Core.Controllers
{
    public abstract class BaseController : Controller
    {
        public User CurrentUser() {
            return AuthService.GetCurrentUser(HttpContext);
        }

        public string GetParameter(string name) {
            return (HasParameter(name)) ? HttpContext.Request.Query[name].First() : null;
        }

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

        public bool HasParameter(string name) {
            return HttpContext.Request.Query.ContainsKey(name);
        }
    }
}
