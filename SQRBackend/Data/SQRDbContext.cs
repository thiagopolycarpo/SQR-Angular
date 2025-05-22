using Microsoft.EntityFrameworkCore;
using SQRBackend.Models;

namespace SQRBackend.Data
{
    public class SQRDbContext : DbContext
    {
        public SQRDbContext(DbContextOptions<SQRDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<ProductMaterial> ProductMaterials { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Production> Productions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasKey(o => o.OrderId);
            modelBuilder.Entity<Product>()
                .HasKey(p => p.ProductCode);
            modelBuilder.Entity<Material>()
                .HasKey(m => m.MaterialCode);
            modelBuilder.Entity<ProductMaterial>()
                .HasKey(pm => new { pm.ProductCode, pm.MaterialCode });
            modelBuilder.Entity<User>()
                .HasKey(u => u.Email);
            modelBuilder.Entity<Production>()
                .HasKey(p => new { p.Id, p.Email, p.OrderId, p.Date });

            // Explicitly configure Order -> Product relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany()
                .HasForeignKey(o => o.ProductCode)
                .HasPrincipalKey(p => p.ProductCode);

            modelBuilder.Entity<ProductMaterial>()
                .HasOne(pm => pm.Product)
                .WithMany()
                .HasForeignKey(pm => pm.ProductCode);
            modelBuilder.Entity<ProductMaterial>()
                .HasOne(pm => pm.Material)
                .WithMany()
                .HasForeignKey(pm => pm.MaterialCode);
            modelBuilder.Entity<Production>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.Email);
            modelBuilder.Entity<Production>()
                .HasOne(p => p.Order)
                .WithMany()
                .HasForeignKey(p => p.OrderId);

            // Seed data
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Email = "teste@sqr.com.br",
                    Name = "Test User",
                    InitialDate = DateTime.Parse("2021-01-01"),
                    EndDate = DateTime.Parse("2025-12-31")
                }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { ProductCode = "abc", ProductDescription = "xxx", Image = "0x000001", CycleTime = 30.3m },
                new Product { ProductCode = "def", ProductDescription = "yyy", Image = "0x00000", CycleTime = 45.5m }
            );

            modelBuilder.Entity<Order>().HasData(
                new Order { OrderId = "111", Quantity = 100.00m, ProductCode = "abc" },
                new Order { OrderId = "222", Quantity = 200.00m, ProductCode = "def" }
            );

            modelBuilder.Entity<Material>().HasData(
                new Material { MaterialCode = "aaa", MaterialDescription = "desc1" },
                new Material { MaterialCode = "bbb", MaterialDescription = "desc2" },
                new Material { MaterialCode = "ccc", MaterialDescription = "desc3" }
            );

            modelBuilder.Entity<ProductMaterial>().HasData(
                new ProductMaterial { ProductCode = "abc", MaterialCode = "aaa" },
                new ProductMaterial { ProductCode = "abc", MaterialCode = "bbb" },
                new ProductMaterial { ProductCode = "def", MaterialCode = "ccc" },
                new ProductMaterial { ProductCode = "def", MaterialCode = "bbb" }
            );
        }
    }
}