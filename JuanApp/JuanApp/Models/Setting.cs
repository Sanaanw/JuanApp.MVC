﻿using System.ComponentModel.DataAnnotations;

namespace JuanApp.Models
{
    public class Setting
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
