using JuanApp.Data;
using JuanApp.Models;
using JuanApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace JuanApp.Controllers
{
    public class BasketController(
        JuanAppContext context,
        UserManager<AppUser> userManager
        ) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddToBasket(int? id)
        {
            if (id == null)
                return NotFound();
            var product = context.Product
                .Include(x=>x.ProductImages)
                .FirstOrDefault(x => x.Id == id);
            if (product == null)
                return NotFound();

            List<BasketItemVm> baskets;
            var basket = HttpContext.Request.Cookies["basket"];
            if (basket != null)
            {
                baskets = JsonConvert.DeserializeObject<List<BasketItemVm>>(basket);
            }
            else
            {
                baskets = new();
            }

            var existBook = baskets.FirstOrDefault(b => b.productId == id);
            if (existBook != null)
            {
                existBook.Count++;
            }
            else
            {
                BasketItemVm basketItem = new()
                {
                    productId = product.Id,
                    Name = product.Name,
                    MainImage = product.ProductImages.FirstOrDefault(bi => bi.Status == true).Name,
                    Price = product.Price,
                    Count = 1
                };
                baskets.Add(basketItem);
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(baskets));
            return Json(baskets);
        }
    }
}
