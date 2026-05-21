using Microsoft.EntityFrameworkCore;

namespace EC_Order_Service.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Models.Commande> Commandes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            string database_name = "EC_Product_Service";
            dbContextOptionsBuilder.UseSqlServer($"{connectionString}Database={database_name};");
        }
    }
}
