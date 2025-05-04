using JuanApp.Data;
using JuanApp.Models;
using JuanApp.Models.Home.Product;
using JuanApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.ProjectModel;
using System.Net;

namespace JuanApp.Controllers
{
    public class ProductController
        (
        JuanAppContext context,
        UserManager<AppUser> userManager
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
                        .Include(p => p.ProductComments)
                        .ThenInclude(b => b.AppUser)
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
            if (releatedProducts == null)
            {
                return NotFound();
            }
            var productDetailVm = new ProductDetailVm
            {
                Product = product,
                ReleatedProducts = releatedProducts
            };
            var userId = userManager.GetUserId(User);
            if (userId != null)
            {
                productDetailVm.HasComment = context.ProductComment
                    .Any(bc => bc.AppUserId == userId && bc.ProductId == id && bc.Status != CommentStatus.Rejected);
            }


            productDetailVm.TotalComments = context.ProductComment
                .Count(bc => bc.ProductId == id && bc.Status != CommentStatus.Rejected);

            var rates = context.ProductComment
                .Where(bc => bc.ProductId == id && bc.Status != CommentStatus.Rejected)
                .Select(bc => (decimal?)bc.Rate)
                .ToList();
            productDetailVm.AvgRate = rates.Any() ? rates.Average() ?? 0 : 0;
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
        private ProductDetailVm GetProductDetailVm(int productId, string userId = null)
        {
            var ExistProduct = context.Product
                .Include(b => b.Category)
                .Include(a => a.ProductTags)
                    .ThenInclude(pt => pt.Tag)
                .Include(b => b.ProductColors)
                    .ThenInclude(pc => pc.Color)
                .Include(bi => bi.ProductImages)
                .Include(b => b.ProductComments)
                .FirstOrDefault(x => x.Id == productId);

            if (ExistProduct == null)
                return null;

            ProductDetailVm productDetailVm = new()
            {
                Product = ExistProduct,
                ReleatedProducts = context.Product
                    .Include(b => b.Category)
                    .Include(a => a.ProductTags)
                        .ThenInclude(pt => pt.Tag)
                    .Include(b => b.ProductColors)
                        .ThenInclude(pc => pc.Color)
                    .Include(bi => bi.ProductImages)
                    .Where(b => b.CategoryId == ExistProduct.CategoryId && b.Id != productId)
                    .ToList(),
            };

            if (userId != null)
            {
                productDetailVm.HasComment = context.ProductComment
                    .Any(bc => bc.AppUserId == userId && bc.ProductId == productId && bc.Status != CommentStatus.Rejected);
            }


            productDetailVm.TotalComments = context.ProductComment
                .Count(bc => bc.ProductId == productId && bc.Status != CommentStatus.Rejected);

            var rates = context.ProductComment
                .Where(bc => bc.ProductId == productId && bc.Status != CommentStatus.Rejected)
                .Select(bc => (decimal?)bc.Rate)
                .ToList();
            productDetailVm.AvgRate = rates.Any() ? rates.Average() ?? 0 : 0;

            return productDetailVm;
        }
        [HttpPost]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> AddComment(ProductComment productComment)
        {

            if (!context.Product.Any(bc => bc.Id == productComment.ProductId))
                return NotFound();
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return BadRequest();
            if (!ModelState.IsValid)
            {
                var vm = GetProductDetailVm(productComment.ProductId, user.Id);
                vm.ProductComment = productComment;
                return View("Detail", vm);
            }

            productComment.AppUserId = userManager.GetUserId(User);
            context.ProductComment.Add(productComment);
            productComment.CreatedDate = DateTime.Now;  
            await context.SaveChangesAsync();

            return RedirectToAction("Detail", "ProductDetail", new { id = productComment.ProductId });

        }
        public async Task<IActionResult> DeleteComment(int? Id)
        {
            if (Id == null)
                return NotFound();
            var existComment = context.ProductComment
                 .Include(b => b.AppUser)
                .FirstOrDefault(b => b.Id == Id);
            if (existComment == null)
                return NotFound();
            if (existComment.AppUserId != userManager.GetUserId(User))
                return NotFound();
            context.ProductComment.Remove(existComment);
            await context.SaveChangesAsync();
            return RedirectToAction("Detail", "Product", new { id = existComment.ProductId });
        }

    }
}
