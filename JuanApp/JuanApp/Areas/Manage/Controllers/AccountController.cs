using JuanApp.Models;
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
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
