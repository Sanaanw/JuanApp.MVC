using JuanApp.Areas.Manage.ViewModels;
using JuanApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JuanApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AccountController
        (
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<AppUser> signInManager
        ) : Controller
    {
        public async Task<IActionResult> CreateAdmin()
        {
            var existingUser = await userManager.FindByNameAsync("admin");
            if (existingUser != null)
                return BadRequest("Admin already exists.");

                await roleManager.CreateAsync(new IdentityRole("Admin"));

            AppUser user = new()
            {
                UserName = "admin",
                FullName = "Admin1",
                Email = "admin@gmail.com"
            };

            var result = await userManager.CreateAsync(user, "_Admin123");

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await userManager.AddToRoleAsync(user, "Admin");

            ViewBag.admin = user.UserName;

            return Content("Admin user created successfully.");
        }
        public async Task<IActionResult> CreateRole()
        {

            await roleManager.CreateAsync(new IdentityRole { Name = "SuperAdmin" });
            await roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            await roleManager.CreateAsync(new IdentityRole { Name = "Member" });
            return Content("Roles Created");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginVm adminLoginVm, string returnUrl)
        {
            if (!ModelState.IsValid) return View();

            var user = await userManager.FindByNameAsync(adminLoginVm.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }

            if (!await userManager.IsInRoleAsync(user, "SuperAdmin") && !await userManager.IsInRoleAsync(user, "Admin"))
            {
                ModelState.AddModelError("", "You are not allowed to login");
                return View();
            }

            var result = await userManager.CheckPasswordAsync(user, adminLoginVm.Password);
            if (!result)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }
            await signInManager.SignInAsync(user, true);

            return RedirectToAction("Index","Dashboard");

        }
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
