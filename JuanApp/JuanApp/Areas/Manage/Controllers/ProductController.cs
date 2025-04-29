using JuanApp.Data;
using JuanApp.Helpers;
using JuanApp.Models.Home.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JuanApp.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        private readonly JuanAppContext context;
        private readonly IWebHostEnvironment env;
        public ProductController(JuanAppContext _context, IWebHostEnvironment _env)
        {
            context = _context;
            env = _env;
        }
        public IActionResult Index(int page = 1, int take = 2)
        {
            var query = context.Product
                .Include(x=>x.ProductImages)
                .AsQueryable();
            PaginatedList<Product> paginatedList = PaginatedList<Product>.Create(query, page, take);
            return View(paginatedList);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(context.Category.ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            ViewBag.Categories = new SelectList(context.Category.ToList(), "Id", "Name");

            if (!context.Category.Any(x => x.Id == product.CategoryId))
            {
                ModelState.AddModelError("Category", "Category is required");
                return View();
            }
            if (product.Files != null)
            {
                foreach (var file in product.Files)
                {
                    ProductImage productImage = new();
                    productImage.ProductId = product.Id;
                    productImage.Name = file.SaveImage(env.WebRootPath, "assets/img/product");
                    product.ProductImages.Add(productImage);
                }
            }
            else
            {
                ModelState.AddModelError("Files", "Images are required");
                return View();
            }
            if (product.MainFile != null)
            {
                ProductImage productImage = new();
                productImage.Status = true;
                productImage.Name = product.MainFile.SaveImage(env.WebRootPath, "assets/img/product");
                product.ProductImages.Add(productImage);
            }
            else
            {
                ModelState.AddModelError("MainFile", "Main image is required");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            context.Product.Add(product);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
