using MaBoutique.Carts.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaBoutique.Carts.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Panier> Paniers { get; set; }
    public DbSet<ArticlePanier> ArticlesPaniers { get; set; }
}
