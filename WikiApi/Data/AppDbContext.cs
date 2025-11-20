using CNSAWiki.Models;
using Microsoft.EntityFrameworkCore;

namespace WikiApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserInfo> Users => Set<UserInfo>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}
