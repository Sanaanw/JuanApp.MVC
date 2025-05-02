using System.ComponentModel.DataAnnotations;

namespace JuanApp.ViewModels
{
    public class UserUpdateProfileVm
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "NewPassword and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
