using System.ComponentModel.DataAnnotations;

namespace JuanApp.Models.Home.Product
{
    public class Order:BaseEntity
    {
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string Phone { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int TotalPrice { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
