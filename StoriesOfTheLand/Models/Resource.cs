using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StoriesOfTheLand.Models
{

    public class Resource
    {
        [Key]
        public int ResourceID { get; set; }

        [Required(ErrorMessage = "Title is mandatory")]
        [StringLength(100, ErrorMessage = "Title must be between 4 and 100 characters.", MinimumLength = 4)]
        public string ResourceTitle { get; set; }

        [StringLength(1000, ErrorMessage = "Description must be between 4 and 1000 characters.", MinimumLength = 4)]
        public string? ResourceDescription { get; set; }

        [Required(ErrorMessage = "URL is mandatory")]
        [RegularExpression("^(https?:\\/\\/).+", ErrorMessage = "Please enter a valid URL. Ensure it starts with http:// or https:// and follows the standard format, such as http://www.example.com.")]
        [StringLength(200, ErrorMessage = "URL must be between 9 and 200 characters.", MinimumLength = 9)]
        public string ResourceURL { get; set; }

        [RegularExpression(".*\\.(jpeg|png|jpg)$", ErrorMessage = "Media file must be of type jpg, jpeg or png")]
        [StringLength(500, ErrorMessage = "Image path must be between 8 and 500 characters.", MinimumLength = 8)]
        public string? ResourceImage { get; set; }

        public FR_Resource? FR_Resource { get; set; }
    }

}
