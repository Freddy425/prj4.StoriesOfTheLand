using System.ComponentModel.DataAnnotations;

namespace StoriesOfTheLand.Models
{
    public class UserImage
    {
        [Key]
        public int UserImageiD { get; set; }

        // Store the learner's IP address
        [Required(ErrorMessage = "IP address is required")]
        [StringLength(15, ErrorMessage = "IP Address can be no larger than 15 characters")]
        public string IP { get; set; }

        [Required(ErrorMessage = "File size is required")]
        [Range(1, 1048576, ErrorMessage = "File Size Must be Above 0 AND less than 1 MB")]
        public int FileSize { get; set; }

        [Required(ErrorMessage = "Date Uploaded is required")]
        public DateTime DateUploaded { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Status is required")]
        public bool status { get; set; } = false;

        [RegularExpression(@"([^\\s]+(\.(?i)(jpeg|jpg|png))$)", ErrorMessage = "Media file must be of type jpeg, jpg, or png")]
        [StringLength(254, ErrorMessage = "Media path must be between 6 and 254 characters", MinimumLength = 6)]
        public string? MediaPath { get; set; }
    }
}

