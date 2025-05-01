using JuanApp.Data;
using JuanApp.Models;
using JuanApp.Services;
using JuanApp.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PustokApp.Services;

namespace JuanApp
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllersWithViews();

            services.AddDbContext<JuanAppContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<LayoutService>();
            services.AddScoped<EmailService>();
            services.Configure<EmailSetting>(config.GetSection("EmailSettings"));
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.User.RequireUniqueEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;
            }).AddEntityFrameworkStores<JuanAppContext>().AddDefaultTokenProviders();

            services.AddControllers()
.AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    opt.JsonSerializerOptions.WriteIndented = true;
});

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Events.OnRedirectToLogin = opt.Events.OnRedirectToAccessDenied = context =>
                {
                    var uri = new Uri(context.RedirectUri);
                    if (context.Request.Path.Value.ToLower().StartsWith("/manage"))
                        context.Response.Redirect("/manage/account/login" + uri.Query);
                    else
                        context.Response.Redirect("/account/login" + uri.Query);
                    return Task.CompletedTask;
                };
            });

        }
    }
}
