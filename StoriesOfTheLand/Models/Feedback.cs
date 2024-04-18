using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace StoriesOfTheLand.Models
{

    public class Feedback
    {
        [Key]
        public int FeedbackID { get; set; }

        [Required(ErrorMessage = "Please leave a name")]
        [StringLength(25, ErrorMessage = "{0} length must be between {2} and {1}", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please leave an email to contact")]
        [StringLength(100, ErrorMessage = "{0} length cannot exceed {1}")]
        [EmailAddress(ErrorMessage = "Email must be a valid email address")]
        public string Email { get; set; }

        [StringLength(500, ErrorMessage = "{0} length cannot exceed {1}")]
        public string? Subject { get; set; }

        public int? SpecimenID { get; set; }

        [Required(ErrorMessage = "Please give us more details")]
        [StringLength(2000, ErrorMessage = "{0} length must be between {2} and {1}", MinimumLength = 30)]
        public string Details { get; set; }

        [EnumRange]
        public Status Status { get; set; } = Status.New;

        [CurrentDateTime(ErrorMessage = "Date must not be in the future")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }

    public enum Status
    {
        New,
        InProgress,
        PendingReponse,
        Resolved,
    }


    public class EnumRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; // Null values are considered valid
            }

            int enumValue = (int)value;

            if (enumValue < 0 || enumValue > 3)
            {
                return new ValidationResult($"Status must be between {0} and {3}.");
            }

            return ValidationResult.Success;
        }
    }
    public class CurrentDateTimeAttribute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {

           
            DateTime dateTimeValue = (DateTime)value;
            DateTime currentDateTime = DateTime.Now;

            // Check if the provided date is close enough to the current date and time
            TimeSpan difference = currentDateTime - dateTimeValue;
            double secondsDifference = (difference.TotalSeconds);
            Console.WriteLine(secondsDifference);
            // Allow a tolerance of 1 second
            return secondsDifference > 0;
  
        }
    }
}