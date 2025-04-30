using PustokApp.Areas.Manage.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanApp.Models.Home.Product
{
    public class Product:BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Required]
        public string Desc { get; set; }
        public bool InStock { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ProductTag> ProductTags { get; set; }
        public List<ProductColor> ProductColors { get; set; }
        public List<ProductImage> ProductImages { get; set; } = new();
        [NotMapped]
        [AllowedType("image/jpeg", "image/png")]
        [AllowedLength(2 * 1024 * 1024)]
        public List<IFormFile> Files { get; set; }
        [NotMapped]
        [AllowedType("image/jpeg", "image/png")]
        [AllowedLength(2 * 1024 * 1024)]
        public IFormFile MainFile { get; set; }
    }
}
