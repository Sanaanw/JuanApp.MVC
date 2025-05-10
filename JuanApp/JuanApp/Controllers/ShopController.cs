using JuanApp.Data;
using JuanApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JuanApp.Controllers
{
    public class ShopController
        (
         JuanAppContext context
        )
        :Controller
    {
        public IActionResult Index(string sort = "AtoZ")
        {
            var query = context.Product
                .Include(x => x.ProductImages.Where(img => img.Status == true))
                .AsQueryable();

            switch (sort)
            {
                case "ZtoA":
                    query = query.OrderByDescending(b => b.Name);
                    break;
                case "PriceAsc":
                    query = query.OrderBy(b => b.Price);
                    break;
                case "PriceDesc":
                    query = query.OrderByDescending(b => b.Price);
                    break;
                default:
                    query = query.OrderBy(b => b.Name);
                    break;
            }

            ViewBag.Sort = sort;
            ShopVm shopVm = new ShopVm
            {
                Products = query.ToList()
            };
            return View(shopVm);
        }

    }
}
