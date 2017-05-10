using Microsoft.AspNetCore.Mvc;
using StarterPack.Models;
using StarterPack.Core.Auth;

namespace StarterPack.Core.Controllers
{
    public abstract partial class BaseController : Controller
    {
        public User CurrentUser() {
            return AuthService.GetCurrentUser(HttpContext);
        }                         
    }
}
