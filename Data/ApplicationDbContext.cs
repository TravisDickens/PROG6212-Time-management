using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeManagementPOE.Models;

namespace TimeManagementPOE.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Modules> Modules { get; set; }
        public DbSet<Semesters> Semesters { get; set; }
        public DbSet<StudyHours> StudyHours { get; set; }   

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}