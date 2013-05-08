using System.ComponentModel.DataAnnotations;
using www.Classes;

namespace www.Models
{
    public class VerifyUserModel
    {
        [Required(ErrorMessage="Error message here.")]
        [RegularExpression(@"^[^,<>\s@]+@([^\s,@\.\[\]]+\.)*[^\s,@\.\[\]]+\.[^\s,@\.\[\]]+$", ErrorMessage = "Invalid Email")]
        //[EmailValidation(ErrorMessage = "Not a valid Email Address")]
        [MaxLength(255)]
        [Display(Name = "Email :")]
        public string Email { get; set; }

        [EnforceTrue(ErrorMessage = "Checkbox Error Message")]
        [Display(Name = "Accept Rules :")]
        public bool AcceptRules { get; set; }
    }
}