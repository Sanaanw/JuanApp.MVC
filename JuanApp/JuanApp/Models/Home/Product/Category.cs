using System.ComponentModel.DataAnnotations;

namespace JuanApp.Models.Home.Product
{
    public class Category:BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public List<Product> Products  { get; set; }
    }
}
