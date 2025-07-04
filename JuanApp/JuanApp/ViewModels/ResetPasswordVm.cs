﻿using System.ComponentModel.DataAnnotations;

namespace JuanApp.ViewModels
{
    public class ResetPasswordVm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }

    }
}
