using Microsoft.EntityFrameworkCore;
using ArtMapApi.Models;

namespace ArtMapApi.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Post> Post { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')");
        }
    }

}