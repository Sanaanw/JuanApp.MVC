using JuanApp.Data;
using JuanApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JuanApp.Controllers
{
    public class ShopController(JuanAppContext context) : Controller
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

            ViewBag.SelectOptions =new List<SelectListItem>
            {
                new SelectListItem(){Text="Name (A - Z)",Value="AtoZ",Selected=sort=="AtoZ"},
                new SelectListItem(){Text="Name (Z - A)",Value="ZtoA",Selected=sort=="ZtoA"},
                new SelectListItem(){Text="Price (Low &gt; High)",Value="PriceAsc",Selected=sort=="PriceAsc"},
                new SelectListItem(){Text="PPrice (High &gt; Low)",Value="PriceDesc",Selected=sort=="PriceDesc"}
            };
            ShopVm shopVm = new ShopVm
            {
                Products = query.ToList()
            };
            return View(shopVm);
        }
    }
}
