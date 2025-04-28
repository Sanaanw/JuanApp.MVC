namespace JuanApp.Models.Home.Product
{
    public class Color:BaseEntity
    {
        public string Name { get; set; }
        public List<ProductColor> ProductColors { get; set; }
    }
}
