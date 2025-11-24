using CNSAWiki.Models;
using Microsoft.EntityFrameworkCore;

namespace WikiApi.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserInfo> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)    
            : base(options)
        {

        }
    }
}
