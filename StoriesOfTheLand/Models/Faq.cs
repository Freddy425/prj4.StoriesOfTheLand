using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace StoriesOfTheLand.Models
{
    public class Faq
    {

        //primary key of Id
        [Key]
        public int Id { get; set; }

        //field Title in instance of database
        [Required(ErrorMessage="Title Is Required")]
        [MinLength(5, ErrorMessage = "Title must be atleast 5 characters long")]
        [MaxLength(200, ErrorMessage ="Title can't be more than 200 characters")]
        public string Title { get; set; }

        //filed of description of instance in database
        [Required(ErrorMessage ="Description Is Required")]
        [MinLength(10, ErrorMessage = "Description must be atleast 10 characters")]
        [MaxLength(500,ErrorMessage ="Description can't be more than 500 characters")]
        public string Description { get; set; }


    }
}
