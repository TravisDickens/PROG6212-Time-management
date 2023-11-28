using System.ComponentModel.DataAnnotations;

namespace TimeManagementPOE.Models
{
    public class Semesters
    {
        [Key]
        public int SemesterId { get; set; }
        public string Name { get; set; }
        public int Weeks { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UserId { get; set; }
    }
}
