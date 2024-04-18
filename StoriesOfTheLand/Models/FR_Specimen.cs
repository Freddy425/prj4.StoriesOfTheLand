using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace StoriesOfTheLand.Models
{
    //validator class that test to see if there is any non-letter attributes
    //in the EnglishName
    

    public class FR_Specimen
    {
        [Key]
        public int SpecimenID { get; set; }

        [Required(ErrorMessage = "FR_SpecimenDescription is mandatory.")]
        [StringLength(5000, ErrorMessage = "{0} length must be between {2} and {1}", MinimumLength = 5)]
        public string FR_SpecimenDescription { get; set; }

        [Required(ErrorMessage = "FR_English Name is required")]
        [MaxLength(100, ErrorMessage = "FR_English name is too long must be 100 characters or less")]
        [MinLength(3, ErrorMessage = "FR_English name is too short must be a minimum of 3 characters")]
        public string FR_EnglishName { get; set; }

        [Required(ErrorMessage = "French Cultural Significance is required")]
        [StringLength(3500, MinimumLength = 1, ErrorMessage = "Cultural Significance must have between 1 and 3500 characters")]
        public string FR_CulturalSignificance { get; set; }
    }
}
