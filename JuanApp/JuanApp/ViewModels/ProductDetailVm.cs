using JuanApp.Models.Home.Product;

namespace JuanApp.ViewModels
{
    public class ProductDetailVm
    {
        public Product Product { get; set; }
        public List<Product> ReleatedProducts { get; set; }

    }
}
