using System.ComponentModel.DataAnnotations;

namespace TimeManagementPOE.Models
{
    public class StudyHours
    {
        [Key]
        public int StudyHourId { get; set; }
        public int ModuleId { get; set; }
        public Modules Module { get; set; }
        public DateTime Date { get; set; }
        public double Hours { get; set; }
        public string UserId { get; set; }
        // Calculate the week number based on the Date property
        public int WeekNumber
        {
            get
            {
                // Assuming each week has 7 days
                return (int)Math.Ceiling(Date.DayOfYear / 7.0);
            }
        }
    }
}

