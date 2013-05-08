using System.ComponentModel.DataAnnotations;
using System.Web;
using www.Classes;

namespace www.Models
{
    public class UploadViewModels
    {
        [Required]
        public int userID { get; set; }

        [Required]
        public int contestID { get; set; }

        [Required]
        public HttpPostedFileBase filename { get; set; }

        [Required]
        public string caption { get; set; }

        public string description { get; set; }

        public string location { get; set; }

        [EnforceTrue(ErrorMessage = "Parent or legal guardian concent is required.")]
        public bool ConcentGuardian { get; set; }

        [EnforceTrue(ErrorMessage = "Permission to publish from all individual(s) is required.")]
        public bool Concent19 { get; set; }

        public string alertMsg { get; set; }

    }
}