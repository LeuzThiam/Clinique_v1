using Microsoft.EntityFrameworkCore;

namespace EC_Product_Service.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Produits => Set<Product>();

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            if (!dbContextOptionsBuilder.IsConfigured)
            {
                dbContextOptionsBuilder.UseSqlServer(
                    "Server=(localdb)\\mssqllocaldb;Database=ec_product_db;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.Nom)
                .HasMaxLength(200);

            modelBuilder.Entity<Product>()
                .Property(p => p.Prix)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.CategorieNom)
                .HasMaxLength(100);
        }
    }
}
