namespace JuanApp.Models.Home.Product
{
    public class ProductImage:BaseEntity
    {
        public string Name { get; set; }
        public bool? Status { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
