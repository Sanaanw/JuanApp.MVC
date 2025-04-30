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
            if (!ModelState.IsValid)
                return View();
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

            context.Product.Add(product);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var existProduct = context.Product
                .Include(x => x.ProductImages)
                .FirstOrDefault(p => p.Id == id);

            if (existProduct == null) return NotFound();

            foreach (var image in existProduct.ProductImages)
            {
                var imagePath = Path.Combine(env.WebRootPath, "assets/img/product", image.Name);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            context.Product.Remove(existProduct);
            context.SaveChanges();

            return Ok();

        }

        [HttpPost]
        public IActionResult DeleteImage(int? id)
        {
            var image = context.ProductImage.FirstOrDefault(x => x.Id == id);
            if (image == null) return NotFound();

            string path = Path.Combine(env.WebRootPath, "assets/img/product", image.Name);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            context.ProductImage.Remove(image);
            context.SaveChanges();

            return Ok();
        }
        public IActionResult Detail(int? id)
        {
            var existProduct = context.Product
               .Include(x => x.ProductImages)
               .FirstOrDefault(b => b.Id == id);
            if (existProduct is null) return NotFound();
            return View(existProduct);
        }
        public IActionResult Edit(int? id)
        {
            ViewBag.Categories = new SelectList(context.Category.ToList(), "Id", "Name");
            if (id == null) return NotFound();
            var product=context.Product
                .Include(x=>x.Category)
                .Include(x => x.ProductImages)
                .Include(x => x.ProductTags)
                .ThenInclude(x => x.Tag)
                .Include(x => x.ProductColors)
                .ThenInclude(x => x.Color)
                .FirstOrDefault(x => x.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(context.Category, "Id", "Name", product.CategoryId);
                return View(product);
            }

            var existProduct = context.Product
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.Id == product.Id);

            if (existProduct == null) return NotFound();

            if (product.MainFile == null && !existProduct.ProductImages.Any(p => p.Status==true))
            {
                ModelState.AddModelError("MainFile", "Main image is required.");
                ViewBag.Categories = new SelectList(context.Category, "Id", "Name", product.CategoryId);
                return View(product);
            }

            if (product.MainFile != null)
            {
                var oldMain = existProduct.ProductImages.FirstOrDefault(x => x.Status == true);
                if (oldMain != null)
                {
                    string oldPath = Path.Combine(env.WebRootPath, "assets/img/product", oldMain.Name);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);

                    context.ProductImage.Remove(oldMain);
                }

                var newMain = new ProductImage
                {
                    Status = true,
                    Name = product.MainFile.SaveImage(env.WebRootPath, "assets/img/product"),
                    ProductId = product.Id
                };

                context.ProductImage.Add(newMain);
            }

            if (product.Files != null)
            {
                foreach (var file in product.Files)
                {
                    var newImage = new ProductImage
                    {
                        Status = false,
                        Name = file.SaveImage(env.WebRootPath, "assets/img/product"),
                        ProductId = product.Id
                    };
                    context.ProductImage.Add(newImage);
                }
            }

            existProduct.Name = product.Name;
            existProduct.Desc = product.Desc;
            existProduct.Price = product.Price;
            existProduct.InStock = product.InStock;
            existProduct.CategoryId = product.CategoryId;
            existProduct.UpdatedDate = DateTime.Now;

            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


    }
}
