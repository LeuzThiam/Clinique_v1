using Microsoft.EntityFrameworkCore;

namespace MaBoutique.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Panier> Paniers { get; set; }
        public DbSet<ArticlePanier> ArticlesPaniers { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<ArticleCommande> ArticlesCommandes { get; set; }
        public DbSet<Facture> Factures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Email unique pour l'utilisateur
            modelBuilder.Entity<Utilisateur>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Relation 1-1 entre Commande et Facture
            modelBuilder.Entity<Commande>()
                .HasOne(c => c.Facture)
                .WithOne(f => f.Commande)
                .HasForeignKey<Facture>(f => f.CommandeId);

            // Relations explicites pour eviter les shadow foreign keys generees par convention.
            modelBuilder.Entity<Panier>()
                .HasOne(p => p.Utilisateur)
                .WithMany(u => u.Paniers)
                .HasForeignKey(p => p.IdUtilisateur);

            modelBuilder.Entity<Commande>()
                .HasOne(c => c.Utilisateur)
                .WithMany(u => u.Commandes)
                .HasForeignKey(c => c.UtilisateurId);

            modelBuilder.Entity<Facture>()
                .HasOne(f => f.Utilisateur)
                .WithMany(u => u.Factures)
                .HasForeignKey(f => f.UtilisateurId);

            modelBuilder.Entity<Produit>()
                .HasOne(p => p.Vendeur)
                .WithMany(u => u.Produits)
                .HasForeignKey(p => p.VendeurId);

            // Relation Produit -> CategorieNom stockée comme string (DummyJSON)
            modelBuilder.Entity<Produit>()
                .Property(p => p.CategorieNom)
                .HasMaxLength(100);

            // Jointure ArticleCommande pour Produit
            modelBuilder.Entity<ArticleCommande>()
                .HasOne(ac => ac.Produit)
                .WithMany(p => p.ArticlesCommandes)
                .HasForeignKey(ac => ac.ProduitId);

            // Jointure ArticlePanier pour Produit
            modelBuilder.Entity<ArticlePanier>()
                .HasOne(ap => ap.Produit)
                .WithMany(p => p.ArticlesPaniers)
                .HasForeignKey(ap => ap.ProduitId);
        }
    }
}
