using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace StoriesOfTheLand.Models
{
    //validator class that test to see if there is any non-letter attributes
    //in the EnglishName
    

    public class Specimen
    {
        //make specimen validatable object

        [Key]
        public int SpecimenID { get; set; }

        [Required(ErrorMessage = "Latin Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot be more than 50 characters")]
        public string LatinName { get; set; }

        [Required(ErrorMessage = "{0} cannot be blank")]
        [StringLength(5000, ErrorMessage = "{0} length must be between {2} and {1}", MinimumLength = 10)]
        public string SpecimenDescription { get; set; }

        [Required(ErrorMessage = "English Name is required")]
        //[NonLetter]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "English Name should not contain numbers or special characters.")]
        [MaxLength(50, ErrorMessage = "English name is too long must be 50 characters or less")]
        [MinLength(3, ErrorMessage = "English name is too short must be a minimum of 3 characters")]
        public string EnglishName { get; set; }

        [RegularExpression("^[a-zA-Zâāéêēîīôōõûū\\s()]*$", ErrorMessage = "Characters are not valid")]
        [MaxLength(90, ErrorMessage = "Cree name must be up to 90 characters")]
        public string? CreeName { get; set; }

        [Required(ErrorMessage = "Cultural Significance is required")]
        [StringLength(3500, MinimumLength = 1, ErrorMessage = "Cultural Significance must have between 1 and 3500 characters")]
        /* A required string that holds the specimen's cultural significance. This can be a long
         * paragraph or paragraphs and has only length as a constraint */
        public string CulturalSignificance { get; set; }

        [Range(53, 54, ErrorMessage = "Latitude must be between 53 and 54")]
        [AllowNull]
        public double? Latitude { get; set; }

        [Range(-106, -105, ErrorMessage = "Longitude must be between -105 and -106")]
        [AllowNull]
        public double? Longitude { get; set; }

        public ICollection<Media>? MediaList { get; set; }


        public FR_Specimen? FR_Specimen { get; set; }

    }
}

//500000 *easting* upper = longitude -105
//434451 *easting* lower = longitude -106


//5872271 *northing lower = 53.0000004 latitude
//5983984 *northing upper = 54 latitude
