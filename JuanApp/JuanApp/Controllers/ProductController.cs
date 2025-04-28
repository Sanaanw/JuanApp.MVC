using JuanApp.Data;
using JuanApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.ProjectModel;

namespace JuanApp.Controllers
{
    public class ProductController
        (
        JuanAppContext context
        ) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ProductDetail(int? id)
        {
            if (id == null)
                return NotFound();
            var product = context.Product
               .Include(p => p.Category)
                .Include(p => p.ProductTags)
                     .ThenInclude(pt => pt.Tag)
                     .Include(p => p.ProductColors)
                        .ThenInclude(pc => pc.Color)
                        .Include(p => p.ProductImages)
                        .FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();
            var releatedProducts = context.Product
                .Include(p => p.Category)
                .Include(p => p.ProductTags)
                    .ThenInclude(pt => pt.Tag)
                    .Include(p => p.ProductColors)
                        .ThenInclude(pc => pc.Color)
                        .Include(p => p.ProductImages)
                        .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id)
                        .ToList();
            if (releatedProducts==null)
            {
                return NotFound();
            }
            var productDetailVm = new ProductDetailVm
            {
                Product = product,
                ReleatedProducts = releatedProducts
            };
            return View(productDetailVm);
        }
        public IActionResult BlogDetail(int? id)
        {
            if (id == null)
                return NotFound();
            var product = context.Product
               .Include(p => p.Category)
                .Include(p => p.ProductTags)
                     .ThenInclude(pt => pt.Tag)
                     .Include(p => p.ProductColors)
                        .ThenInclude(pc => pc.Color)
                        .Include(p => p.ProductImages)
                        .FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();
            var recentProducts = context.Product
                .Include(p => p.Category)
                .Include(p => p.ProductTags)
                    .ThenInclude(pt => pt.Tag)
                    .Include(p => p.ProductColors)
                        .ThenInclude(pc => pc.Color)
                        .Include(p => p.ProductImages)
                        .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id)
                        .ToList();
            if (recentProducts == null)
            {
                return NotFound();
            }
            var products = context.Product
                .Include(p => p.Category)
                .Include(p => p.ProductTags)
                .ThenInclude(pt => pt.Tag)
                .ToList();
            var blogDetailVm = new BlogDetailVm
            {
                Product = product,
                RecentPosts = recentProducts,
                ProductList=products
            };
            return View(blogDetailVm);
        }
       
    }
}
