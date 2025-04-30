using System.ComponentModel.DataAnnotations;

namespace JuanApp.Areas.Manage.ViewModels
{
    public class AdminLoginVm
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
