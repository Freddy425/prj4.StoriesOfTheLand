using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StoriesOfTheLand.Models
{

    public class FR_Resource
    {
        [Key]
        public int ResourceID { get; set; }

        [Required(ErrorMessage = "Title is mandatory")]
        [StringLength(200, ErrorMessage = "Title must be between 3 and 200 characters.", MinimumLength = 3)]
        public string FR_ResourceTitle { get; set; }

        [StringLength(2000, ErrorMessage = "Description must be between 3 and 2000 characters.", MinimumLength = 3)]
        public string? FR_ResourceDescription { get; set; }

    }

}
