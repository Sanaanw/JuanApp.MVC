using JuanApp.Models.Home.Product;

namespace JuanApp.ViewModels
{
    public class BlogDetailVm
    {
        public Product Product { get; set; }
        public List<Product> RecentPosts { get; set; }
        public List<Product> ProductList { get; set; }
    }
}
