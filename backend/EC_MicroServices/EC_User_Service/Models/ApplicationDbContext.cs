using Microsoft.EntityFrameworkCore;

namespace EC_User_Service.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Models.User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=FalseTrust Server Certificate=False;Application Intent=ReadWrite;";
            string databaseName = "EC_User_Service";
            dbContextOptionsBuilder.UseSqlServer($"{connectionString};Database={databaseName};");
        }

    }
}
