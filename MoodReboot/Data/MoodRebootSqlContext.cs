using Microsoft.EntityFrameworkCore;
using MoodReboot.Models;

namespace MoodReboot.Data
{
    public class MoodRebootSqlContext : DbContext
    {
        public MoodRebootSqlContext(DbContextOptions<MoodRebootSqlContext> options) : base(options) { }
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Center> Centers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<ContentGroup> ContentGroups { get; set; }
        public DbSet<Content> Contents { get; set; }
    }
}
