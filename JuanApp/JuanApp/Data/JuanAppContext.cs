using JuanApp.Models;
using JuanApp.Models.Home;
using JuanApp.Models.Home.Product;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JuanApp.Data
{
    public class JuanAppContext : IdentityDbContext<AppUser>
    {
        public DbSet<Slider> Slider { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ProductTag> Tag { get; set; }
        public DbSet<ProductTag> ProductTag { get; set; }
        public DbSet<Color> Color { get; set; }
        public DbSet<ProductColor> ProductColor { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<Setting> Setting { get; set; }

        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<ProductComment> ProductComment { get; set; }
        public DbSet<DbBasketItem> DbBasketItem { get; set; }

        public JuanAppContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductColor>()
                .HasKey(ps => new { ps.ProductId, ps.ColorId });
            modelBuilder.Entity<ProductTag>()
                .HasKey(ps => new { ps.ProductId, ps.TagId });
        }
        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.CreatedDate = DateTime.Now;
                if (entry.State == EntityState.Modified)
                    entry.Entity.UpdatedDate = DateTime.Now;
            }
            return base.SaveChanges();
        }
    }
}
