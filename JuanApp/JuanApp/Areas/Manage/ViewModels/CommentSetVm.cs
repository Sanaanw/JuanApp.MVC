using JuanApp.Models.Home.Product;

namespace JuanApp.Areas.Manage.ViewModels
{
    public class CommentSetVm
    {
        public List<ProductComment> ProductComments { get; set; }
        public List<Product> Products { get; set; }
    }
}
