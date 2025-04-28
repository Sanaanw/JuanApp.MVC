using JuanApp.Data;
using JuanApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokApp.Services;

namespace JuanApp.Controllers;

public class HomeController(JuanAppContext context,LayoutService layoutService) : Controller
{

    public IActionResult Index()
    {
        var products = context.Product
            .Include(p => p.Category)
            .Include(p => p.ProductTags)
                .ThenInclude(pt => pt.Tag)
                .Include(p => p.ProductColors)
                .ThenInclude(pc => pc.Color)
                .Include(p => p.ProductImages)
            .ToList();
        var sliders = context.Slider.ToList();
        var homeVm = new HomeVm
        {
            Sliders = sliders,
            Products = products
        };
        return View(homeVm);
    }

}
