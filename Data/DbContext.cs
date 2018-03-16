using Microsoft.EntityFrameworkCore;
using ArtMapApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ArtMapApi.Data
{
    public class ApplicationDbContext: IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Post> Post { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Like> Like { get; set; }
        public DbSet<Comment> Comment { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("strftime('%Y-%m-%d %H:%M:%S')");

            base.OnModelCreating(modelBuilder);
        }
    }

}