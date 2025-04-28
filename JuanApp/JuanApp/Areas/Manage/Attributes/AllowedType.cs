using System.ComponentModel.DataAnnotations;

namespace PustokApp.Areas.Manage.Attributes
{
    public class AllowedType : ValidationAttribute
    {
        private readonly string[] _types;
        public AllowedType(params string[] types)
        {
            _types = types;
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
                if (!_types.Contains(f.ContentType))
                {
                    return new ValidationResult("File type isn't valid");
                }
            }

            return ValidationResult.Success;
        }
    }
}
