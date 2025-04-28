using System.ComponentModel.DataAnnotations;

namespace JuanApp.Models
{
    public class BaseEntity
    {
        [Required]
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; } 
        public DateTime? UpdatedDate { get; set; } 
    }
}
