namespace JuanApp.Models.Home.Product
{
    public class DbBasketItem:BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int Count { get; set; }
    }
}
