using Microsoft.AspNetCore.Mvc;
using StarterPack.Models;
using StarterPack.Core.Helpers;
using System;
using StarterPack.Core.Auth;

namespace StarterPack.Core.Controllers
{
    public abstract partial class BaseController : Controller
    {           
        public BaseController(IServiceProvider serviceProvider) {
            Services.Instance = serviceProvider;   
        }

        public User CurrentUser() {
            return AuthService.GetCurrentUser(HttpContext);
        }                         
    }
}
