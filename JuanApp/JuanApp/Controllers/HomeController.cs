using JuanApp.Data;
using JuanApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JuanApp.Controllers;

public class HomeController(JuanAppContext context) : Controller
{

    public IActionResult Index()
    {
        var sliders = context.Slider.ToList();
        var homeVm = new HomeVm
        {
            Sliders = sliders
        };
        return View(homeVm);
    }

}
