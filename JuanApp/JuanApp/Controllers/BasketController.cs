﻿using JuanApp.Data;
using JuanApp.Models;
using JuanApp.Models.Home.Product;
using JuanApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
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
            var basket = HttpContext.Request.Cookies["basket"];
            List<BasketItemVm> basketItemVmList;
            if (basket != null)
                basketItemVmList = JsonConvert.DeserializeObject<List<BasketItemVm>>(basket);
            else
            {
                basketItemVmList = new();

            }
            foreach (var item in basketItemVmList)
            {
                var book = context.Product
                    .Include(b => b.ProductImages)
                    .FirstOrDefault(b => b.Id == item.productId);
                item.MainImage = book.ProductImages
                    .FirstOrDefault(bi => bi.Status == true).Name;
                item.Price = book.Price;
                item.Name = book.Name;
            }
            return View(basketItemVmList);
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

            if (User.Identity.IsAuthenticated)
            {
                var user = userManager.Users
                    .Include(b => b.DbBasketItems)
                    .FirstOrDefault(u => u.UserName == User.Identity.Name);
                var existUserBasketItem = user.DbBasketItems
                    .FirstOrDefault(b => b.ProductId == id);
                if (existUserBasketItem != null)
                {
                    existUserBasketItem.Count++;
                }
                else
                {
                    user.DbBasketItems.Add(new DbBasketItem
                    {
                        ProductId = product.Id,
                        Count = 1,
                        AppUserId = user.Id
                    });
                }
                context.SaveChanges();
            }

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(baskets));
            return Json(baskets);
        }
        public IActionResult DeleteFromBasket(int? id)
        {
            if (id == null)
                return NotFound();

            var product = context.Product.FirstOrDefault(x => x.Id == id);
            if (product == null)
                return NotFound();

            var basket = HttpContext.Request.Cookies["basket"];
            List<BasketItemVm> baskets = basket != null
                ? JsonConvert.DeserializeObject<List<BasketItemVm>>(basket)
                : new List<BasketItemVm>();

            var existItem = baskets.FirstOrDefault(b => b.productId == id);
            if (existItem != null)
            {
                baskets.Remove(existItem);
            }

            if (User.Identity.IsAuthenticated)
            {
                var user = userManager.Users
                    .Include(u => u.DbBasketItems)
                    .FirstOrDefault(u => u.UserName == User.Identity.Name);

                var userBasketItem = user?.DbBasketItems.FirstOrDefault(b => b.ProductId == id);
                if (userBasketItem != null)
                {
                    context.DbBasketItem.Remove(userBasketItem);
                    context.SaveChanges();
                }
            }

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(baskets));
            return RedirectToAction("Index","Basket");
        }

    }
}
