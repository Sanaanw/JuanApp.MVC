using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JuanApp.Areas.Manage.Controllers
{
    public class DashBoardController : Controller
    {
        [Area("Manage")]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Index()
        {
            return View();

        }
    }
}
