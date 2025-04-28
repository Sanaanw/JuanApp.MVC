using System.ComponentModel.DataAnnotations;

namespace PustokApp.Areas.Manage.Attributes
{
    public class AllowedLength : ValidationAttribute
    {
        private readonly int _length;
        public AllowedLength(int length)
        {
            _length = length;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            List<IFormFile> files = new();
            if (value is List<IFormFile> fileList)
                files = fileList;
            if (value is IFormFile file)
                files.Add(file);
            foreach (var f in files)
            {
                if (f.Length > _length)
                {
                    return new ValidationResult($"Max length is {_length}");
                }
            }
            return ValidationResult.Success;
        }
    }
}
