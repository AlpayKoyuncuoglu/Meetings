using DocMeetingPro.Models;
using Microsoft.EntityFrameworkCore;

namespace DocMeetingPro.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Saloon> Saloons { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
