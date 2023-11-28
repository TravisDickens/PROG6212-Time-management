using System.ComponentModel.DataAnnotations;

namespace TimeManagementPOE.Models
{
    public class Modules
    {
        [Key]
        public int ModuleId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public int ClassHours { get; set; }
        public string Semester { get; set; }
        public int StudyHoursLeft { get; set; }
        public string UserId { get; set; }

    }
}
