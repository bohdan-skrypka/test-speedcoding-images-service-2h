using Microsoft.EntityFrameworkCore;
using Images.Database.Models.Business;

namespace Images.EntityFrameworkContext
{
    public class DatabaseContext : DbContext
    {
        public DbSet<ImageEntity> Images { get; set; }

        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            // ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
