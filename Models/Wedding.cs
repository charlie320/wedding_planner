using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models

{
    public class Wedding : BaseEntity {
        [Key]
        public int WeddingId { get; set; }

        [Required]
        [MinLength(2, ErrorMessage= "Wedder name must be at least 2 characters long.")]
        [Display(Name = "Wedder One")]
        public string WedderOne {get; set;}

        [Required]
        [MinLength(2, ErrorMessage= "Wedder name must be at least 2 characters long.")]
        [Display(Name = "Wedder Two")]
        public string WedderTwo {get; set;}

        [Required]
        [FutureDate]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime Date {get; set;}

        [Required]
        [Display(Name = "Wedding Address")]
        public string WeddingAddress {get; set;}

        public double Latitude {get; set;}

        public double Longitude {get; set;}

        public int PlannerId {get; set;}

        public User Planner {get; set;}

        public DateTime created_at {get; set;}

        public DateTime updated_at {get; set;}

        public List<WeddingConfirmation> GuestsAttending {get; set;}
        
        public Wedding() {
            GuestsAttending = new List<WeddingConfirmation>();
        }

        public class FutureDate : ValidationAttribute {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
                DateTime date = (DateTime)value;
                return date < DateTime.Now ? new ValidationResult("Date must be in the future.") : ValidationResult.Success;
            }
        }

    }
}