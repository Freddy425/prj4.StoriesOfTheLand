using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoriesOfTheLand.Models
{
    
    public class Media
    {
        [Key]
        public int? Id { get; set; }

        [ForeignKey("SpecimenID")]
        public int SpecimenID { get; set; }

        public Specimen Specimen { get; set; }

        [RegularExpression("^(Audio|Image|Video)$", ErrorMessage = "Media type must be Audio, Image, or Video")]
        [StringLength(20, ErrorMessage = "Media Type must be between 5 and 20 characters", MinimumLength = 5)]
        public string MediaType { get; set; }

        [RegularExpression("([^\\s]+(\\.(?i)(m4a|mp3|jpe?g|png))$)", ErrorMessage = "Media file must be of type m4a, mp3, jpeg, or png")]
        [StringLength(254, ErrorMessage = "Media path must be between 6 and 254 characters", MinimumLength = 6)]
        public string? MediaPath { get; set; }
    }
}