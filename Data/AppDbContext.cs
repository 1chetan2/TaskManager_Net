
using JwtApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Usertb> Usertbs { get; set; }
        public DbSet<Student> Students { get; set; }

        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<TaskChild> TaskChilds { get; set; }

    }
}
