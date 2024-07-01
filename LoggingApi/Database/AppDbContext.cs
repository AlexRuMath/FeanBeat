using Microsoft.EntityFrameworkCore;

namespace LoggingApi.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<DataRecord> DataRecords { get; set; } = null!;
        public DbSet<LogModel> Logs { get; set; } = null!;
        public AppDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataRecord>().HasKey(dr => dr.Id);
            modelBuilder.Entity<LogModel>().HasKey(l => l.Id);
        }
    }
}
