using JuanApp.Data;
using JuanApp.Models;
using JuanApp.Services;
using JuanApp.Settings;
using JuanApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JuanApp.Controllers
{
    public class AccountController
        (

        JuanAppContext context,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<AppUser> signInManager,
        EmailService emailService,
        IOptions<EmailSetting> emailSetting
        )
        : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterVm userRegisterVm)
        {
            if (!ModelState.IsValid)
                return View(userRegisterVm);
            AppUser user = await userManager.FindByNameAsync(userRegisterVm.UserName);
            if (user != null)
                ModelState.AddModelError("UserName", "This username is already taken");
            user = new AppUser
            {
                FullName = userRegisterVm.FullName,
                UserName = userRegisterVm.UserName,
                Email = userRegisterVm.Email
            };
            var result = await userManager.CreateAsync(user, userRegisterVm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            await userManager.AddToRoleAsync(user, "Member");
            //send email verification
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var url = Url.Action("VerifyEmail", "Account", new { email = user.Email, token }, Request.Scheme);

            using StreamReader streamReader = new StreamReader("wwwroot/templates/verifyEmail.html");
            string body = await streamReader.ReadToEndAsync();
            body = body.Replace("{{url}}", url);
            body = body.Replace("{{username}}", user.FullName);

            emailService.SendEmail(user.Email, "Email Verification", body, emailSetting.Value);

            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();  
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginVm userLoginVm, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View();
            var user = await userManager.FindByNameAsync(userLoginVm.UserNameOrEmail);
            if (user == null)
            {
                user = await userManager.FindByEmailAsync(userLoginVm.UserNameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid username or email");
                    return View();
                }
            }
            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Email is not confirmed");
                return View();
            }
            if (await userManager.IsInRoleAsync(user, "Admin") || await userManager.IsInRoleAsync(user, "SuperAdmin"))
            {
                ModelState.AddModelError("", "You are not allowed to login");
                return View();
            }

            var result = await signInManager.PasswordSignInAsync(user, userLoginVm.Password, userLoginVm.RememberMe, true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Your account is locked out");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }

            //Response.Cookies.Delete("basket");
            return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("Index", "Home");
        }
        //Forget Password
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVm forgetPasswordVm)
        {
            if (!ModelState.IsValid)
                return View();
            var user = await userManager.FindByEmailAsync(forgetPasswordVm.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email not found");
                return View();
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action("ResetPassword", "Account", new { email = user.Email, token }, Request.Scheme);

            using StreamReader streamReader = new StreamReader("wwwroot/templates/forgotpassword.html");
            string body = await streamReader.ReadToEndAsync();
            body = body.Replace("{{url}}", url);
            body = body.Replace("{{username}}", user.FullName);

            emailService.SendEmail(user.Email, "Reset Password", body, emailSetting.Value);

            return RedirectToAction("Login", "Account");
        }
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVm resetPasswordVm)
        {
            if (!ModelState.IsValid)
                return View();
            var user = await userManager.FindByEmailAsync(resetPasswordVm.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email not found");
                return View();
            }
            var result = await userManager.ResetPasswordAsync(user, resetPasswordVm.Token, resetPasswordVm.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            return RedirectToAction("Login", "Account");
        }
        //Verify email
        public async Task<IActionResult> VerifyEmail(string email, string token)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
                return NotFound();
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound();
            var result = await userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        //Profile
        [Authorize(Roles = "Member" )]
        public async Task<IActionResult> Profile(string tab = "Dashboard")
        {
            ViewBag.tab = tab;
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();
            var userUpdateProfileVm = new UserUpdateProfileVm
            {
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email
            };
            var userProfileVm = new UserProfileVm
            {
                UserUpdateProfileVm = userUpdateProfileVm,
            };
            return View(userProfileVm);
        }
        [HttpPost]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Profile(UserUpdateProfileVm userUpdateProfileVm, string tab = "Profile")
        {
            ViewBag.tab = tab;
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();
            UserProfileVm userProfileVm = new UserProfileVm
            {
                UserUpdateProfileVm = userUpdateProfileVm,
            };


            if (userUpdateProfileVm.NewPassword != null)
            {
                if (userUpdateProfileVm.CurrentPassword == null)
                {
                    ModelState.AddModelError("CurrentPassword", "Current password is required");
                    return View(userProfileVm);
                }
                else
                {
                    if (userUpdateProfileVm.NewPassword == null)
                    {
                        ModelState.AddModelError("NewPassword", "New password is required");
                        return View(userProfileVm);
                    }
                    var passwordUpdateResult = await userManager.ChangePasswordAsync(user, userUpdateProfileVm.CurrentPassword, userUpdateProfileVm.NewPassword);
                    if (!passwordUpdateResult.Succeeded)
                    {
                        foreach (var error in passwordUpdateResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(userProfileVm);
                    }
                }
            }
            user.FullName = userUpdateProfileVm.FullName;
            user.UserName = userUpdateProfileVm.UserName;
            user.Email = userUpdateProfileVm.Email;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(userProfileVm);
            }
            await signInManager.SignInAsync(user, true);
            if (!ModelState.IsValid)
                return View(userProfileVm);
            return RedirectToAction("Index", "Home");
        }
    } 
}
