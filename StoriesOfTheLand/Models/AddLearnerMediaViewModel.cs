using System.ComponentModel.DataAnnotations;

namespace StoriesOfTheLand.Models
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSizeInBytes;

        public MaxFileSizeAttribute(int maxFileSizeInBytes)
        {
            _maxFileSizeInBytes = maxFileSizeInBytes;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IList<IFormFile> files)
            {
                foreach (var file in files)
                {
                    if (file.Length > _maxFileSizeInBytes)
                    {
                        return new ValidationResult($"The file size should not exceed {_maxFileSizeInBytes / 1024} KB.");
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
    public class AllowedFileExtensionsUserImage : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;

        public AllowedFileExtensionsUserImage(string[] allowedExtensions)
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
                            return new ValidationResult("File must be of type png, jpg or jpeg");
                        }
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
    public class MaxFileCountAttribute : ValidationAttribute
    {
        private readonly int _maxFileCount;

        public MaxFileCountAttribute(int maxFileCount)
        {
            _maxFileCount = maxFileCount;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is List<IFormFile> files)
            {
                if (files.Count > _maxFileCount)
                {
                    return new ValidationResult($"Only 1 file can be uploaded");
                }
            }

            return ValidationResult.Success;
        }
    }
    public class AddLearnerMediaViewModel
    {
        [Required(ErrorMessage = "File is required to add media")]
        [AllowedFileExtensionsUserImage(new string[] { ".jpg", ".jpeg", ".png" }, ErrorMessage = "Only files with the following extensions are allowed: .jpg, .jpeg, .png")]
        [MaxFileSize(1024 * 1024)]
        [MaxFileCount(1, ErrorMessage = "Only one file can be uploaded")]
        public IFormFile MediaView { get; set; }
    }
}
