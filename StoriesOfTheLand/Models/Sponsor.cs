using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoriesOfTheLand.Models
{
    public class Sponsor
    {

        [Key]
        public int SponsorID { get; set; }

        [Required(ErrorMessage = "Sponsor Name is required")]
        [StringLength(50, ErrorMessage = "Sponsor Name length must be between {2} and {1}", MinimumLength = 1)]
        public string SponsorName { get; set; }

        [Required(ErrorMessage = "Sponsor URL is required")]
        [Url(ErrorMessage = "Must Be a Valid URL")]
        [StringLength(100, ErrorMessage = "Sponsor Link length must be between {2} and {1}", MinimumLength = 1)]
        public string SponsorURL { get; set; }

        [RegularExpression("([^\\s]+(\\.(?i)(jpe?g|png))$)", ErrorMessage = "Media file must be of type jpeg, or png")]
        public string? SponsorImagePath { get; set; }

     
        
        
        
        
        
  



    }
}
