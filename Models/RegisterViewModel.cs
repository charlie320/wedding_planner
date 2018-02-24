using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models

{
    public class RegisterViewModel : BaseEntity {

        [Required]
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters long.")]
        [Display(Name = "First Name:")]
        public string FirstName {get; set;}

        [Required]
        [MinLength(2, ErrorMessage = "Last name must be at least 2 characters long.")]
        [Display(Name = "Last Name:")]
        public string LastName {get; set;}

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address:")]
        public string Email {get; set;}

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password:")]
        public string Password {get; set;}

        [Compare("Password", ErrorMessage = "Password and confirmation must match.")]
        [Display(Name = "Password Confirmation")]
        public string PasswordConfirmation {get; set;}
        
    }
}