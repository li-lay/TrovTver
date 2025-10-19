using Microsoft.EntityFrameworkCore;
using TrovTver.Models;

namespace TrovTver.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItems> TodoItems { get; set; } = null;
    }
}
