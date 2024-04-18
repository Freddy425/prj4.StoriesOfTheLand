using System.ComponentModel.DataAnnotations;

namespace StoriesOfTheLand.Models
{
    public class AllowedFileExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;

        public AllowedFileExtensionsAttribute(string[] allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is List<IFormFile> files)
            {
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

                        if (!_allowedExtensions.Contains(fileExtension))
                        {
                            return new ValidationResult("File must be of type png, jpeg, m4a, mp3");
                        }
                    }
                }
            }

            return ValidationResult.Success;
        }
    }


    public class AddMediaViewModel
    {

        [Required(ErrorMessage = "File is required to add media")]
        [AllowedFileExtensions(new string[] { ".jpg", ".jpeg", ".png", ".m4a" })]
        public List<IFormFile> MediaFile { get; set; }
    }
}
