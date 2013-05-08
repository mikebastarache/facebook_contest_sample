using System;

namespace www.Models
{
    public class Submission
    {
        public int userID { get; set; }
        public int contestID { get; set; }
        public string filename { get; set; }
        public DateTime? dateCreated { get; set; }
        public string caption { get; set; }
        public string location { get; set; }
        public string description { get; set; }
    }
}
