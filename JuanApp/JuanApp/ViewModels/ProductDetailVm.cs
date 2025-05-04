using JuanApp.Models.Home.Product;

namespace JuanApp.ViewModels
{
    public class ProductDetailVm
    {
        public Product Product { get; set; }
        public List<Product> ReleatedProducts { get; set; }
        public int TotalComments { get; set; }
        public bool HasComment { get; set; }
        public decimal AvgRate { get; set; }
        public ProductComment ProductComment { get; set; }

    }
}
