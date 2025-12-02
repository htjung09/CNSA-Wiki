using CNSAWiki.Models;
using Microsoft.EntityFrameworkCore;

namespace WikiApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // 기존 Users 유지
        public DbSet<UserInfo> Users { get; set; }

        // 새로 추가하는 WikiPages
        public DbSet<WikiPage> WikiPages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // WikiPage 최소 설정
            modelBuilder.Entity<WikiPage>(entity =>
            {
                entity.HasKey(e => e.PageId);

                entity.Property(e => e.Title)
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(e => e.Content)
                      .IsRequired();

                // 필요하면 CreatedAt/UpdatedAt에 기본값 설정 가능 (DB 쪽에서)
                // entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                // entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // 기존 UserInfo 설정이 있으면 여기에 추가하거나 유지
        }
    }
}
