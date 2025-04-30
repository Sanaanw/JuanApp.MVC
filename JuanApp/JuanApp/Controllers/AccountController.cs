using JuanApp.Data;
using JuanApp.Models;
using JuanApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JuanApp.Controllers
{
    public class AccountController
        (

        JuanAppContext context,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<AppUser> signInManager
        )
        : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
    } 
}
