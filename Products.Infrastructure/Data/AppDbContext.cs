using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities; 

namespace Products.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(e =>
            {
                e.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<User>(e =>
            {
                e.HasIndex(u => u.Username).IsUnique();
            });
        }
    }
}
