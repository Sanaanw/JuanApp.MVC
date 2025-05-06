using JuanApp.Models.Home.Product;
using Microsoft.AspNetCore.Identity;

namespace JuanApp.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public bool IsSubcribed { get; set; } =false;
        public List<DbBasketItem> DbBasketItems { get; set; }

    } 
}
