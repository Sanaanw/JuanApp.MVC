using JuanApp.Data;
using JuanApp.Models;
using JuanApp.Models.Home.Product;
using JuanApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JuanApp.Controllers
{
    public class OrderController
        (
        JuanAppContext context,
        UserManager<AppUser> userManager
        )
        : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Member")]
        public IActionResult Checkout()
        {
            var user = userManager.Users
                .Include(x => x.DbBasketItems)
                .ThenInclude(x => x.Product)
                .FirstOrDefault(x => x.UserName == User.Identity.Name);

            var checkoutVm = new CheckOutVm
            {
                CheckOutItems = user.DbBasketItems.Select(x => new CheckOutItemVm
                {
                    Name = x.Product.Name,
                    Count = x.Count,
                    Price = x.Product.Price
                }).ToList(),
                TotalPrice = user.DbBasketItems.Sum(x => x.Count * x.Product.Price),
            };

            return View(checkoutVm);
        }
        [HttpPost]
        [Authorize(Roles = "Member")]
        public IActionResult CheckOut(OrderVm orderVm)
        {

            var user = userManager.Users
               .Include(x => x.DbBasketItems)
               .ThenInclude(x => x.Product)
               .FirstOrDefault(x => x.UserName == User.Identity.Name);

            var checkoutVm = new CheckOutVm
            {
                CheckOutItems = user.DbBasketItems.Select(x => new CheckOutItemVm
                {
                    Name = x.Product.Name,
                    Count = x.Count,
                    Price = x.Product.Price
                }).ToList(),
                TotalPrice = user.DbBasketItems.Sum(x => x.Count * x.Product.Price),
                OrderVm = orderVm
            };
            if (!ModelState.IsValid)
                return View(checkoutVm);

            var order = new Order
            {
                TotalPrice = (int)user.DbBasketItems.Sum(x => x.Count * x.Product.Price),
                Address = orderVm.Address,
                CompanyName=orderVm.CompanyName,
                City = orderVm.City,
                ZipCode = orderVm.ZipCode,
                Phone = orderVm.Phone,
                AppUserId = user.Id,
                OrderItems = user.DbBasketItems.Select(x => new OrderItem
                {
                    ProductId = x.ProductId,
                    Count = x.Count
                }).ToList(),
            };
            context.Order.Add(order);
            context.DbBasketItem.RemoveRange(user.DbBasketItems);
            context.SaveChanges();
            return RedirectToAction("Profile", "Account", new { tab = "Order" });

        }

    }
}
