using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using www.Classes;

namespace www.Models
{
    public class UserViewModels
    {
        [Required]
        [DisplayName("Salutation")]
        public int? Salutation { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Error message here.")]
        [RegularExpression(@"^[^,<>\s@]+@([^\s,@\.\[\]]+\.)*[^\s,@\.\[\]]+\.[^\s,@\.\[\]]+$", ErrorMessage = "Invalid Email")]
        //[EmailValidation(ErrorMessage = "Not a valid Email Address")]
        [MaxLength(255)]
        [Display(Name = "Email :")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Address")]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Required]
        [DisplayName("City")]
        public string City { get; set; }

        [Required]
        [DisplayName("Province")]
        public string Province { get; set; }

        [Required]
        [RegularExpression(@"[abceghjklmnprstvxyABCEGHJKLMNPRSTVXY]{1}\d{1}[A-Za-z]{1} *\d{1}[A-Za-z]{1}\d{1}", ErrorMessage = "Invalid Postal Code")]
        [MaxLength(7)]
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [RegularExpression(@"(1\s*[-\/\.]?)?(\((\d{3})\)|(\d{3}))\s*[-\/\.]?\s*(\d{3})\s*[-\/\.]?\s*(\d{4})\s*(([xX]|[eE][xX][tT])\.?\s*(\d+))*", ErrorMessage = "Invalid Phone")]
        [MaxLength(15)]
        [DisplayName("Telephone")]
        public string Telephone { get; set; }

        [DisplayName("Language")]
        public string Language { get; set; }

        [DisplayName("Opt In")]
        public bool OptIn { get; set; }

        [DisplayName("Year of Birth")]
        public string YearOfBirth { get; set; }

        [EnforceTrue(ErrorMessage = "Checkbox Error Message")]
        [Display(Name = "Accept Rules :")]
        public bool AcceptRules { get; set; }

        public int OriginalFriendId { get; set; }

        public int ContestSignupId { get; set; }

        public string Fbuid { get; set; }
    }
}
