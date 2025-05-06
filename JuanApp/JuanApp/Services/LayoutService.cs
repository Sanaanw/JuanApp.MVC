using JuanApp.Data;
using JuanApp.Models;
using JuanApp.Models.Home.Product;
using JuanApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace PustokApp.Services
{
    public class LayoutService(
        JuanAppContext context,
         IHttpContextAccessor httpContextAccessor,
        UserManager<AppUser> userManager

        )
    {
        public List<Setting> GetSettings()
        {
            return context.Setting.ToList();
        }
        public List<Category> GetCategories()
        {
            return context.Category.ToList();
        }
        public List<BasketItemVm> GetBasket()
        {
            var httpContext = httpContextAccessor.HttpContext;
            var basket = httpContext.Request.Cookies["basket"];
            var basketList = new List<BasketItemVm>();
            if (basket != null)
                basketList = JsonConvert.DeserializeObject<List<BasketItemVm>>(basket);
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var user = userManager.Users
                    .Include(u => u.DbBasketItems)
                    .ThenInclude(b => b.Product)
                    .ThenInclude(b => b.ProductImages)
                    .FirstOrDefault(u => u.UserName == httpContext.User.Identity.Name);
                foreach (var item in user.DbBasketItems)
                {
                    if (!basketList.Any(bi => bi.productId == item.ProductId))
                    {
                        basketList.Add(new BasketItemVm
                        {
                            productId = item.ProductId,
                            Name = item.Product.Name,
                            MainImage = item.Product.ProductImages.FirstOrDefault(x => x.Status == true).Name,
                            Price = item.Product.Price,
                            Count = item.Count
                        });
                    }
                }
                httpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketList));
            }
            foreach (var item in basketList)
            {
                var product = context.Product
                    .Include(b => b.ProductImages)
                    .FirstOrDefault(b => b.Id == item.productId);
                item.MainImage = product.ProductImages
                    .FirstOrDefault(bi => bi.Status == true).Name;
                item.Price = product.Price;
                item.Name = product.Name;

            }
            return basketList;
        }
    }
}
