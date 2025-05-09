using System.ComponentModel.DataAnnotations;

namespace JuanApp.ViewModels
{
    public class OrderVm
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
    }
}
