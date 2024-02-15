using Microsoft.EntityFrameworkCore;

namespace OnlineShop.Data
{
    public class ShopContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.RowVersion)
                .IsRowVersion();
        }
    }
}
