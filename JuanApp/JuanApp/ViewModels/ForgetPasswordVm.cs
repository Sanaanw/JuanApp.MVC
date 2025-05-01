using System.ComponentModel.DataAnnotations;

namespace JuanApp.ViewModels
{
    public class ForgetPasswordVm
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
