using Microsoft.EntityFrameworkCore;
using Platforma.Models;

namespace Platforma.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Firma> Firme { get; set; }
        public DbSet<Recenzie> Recenzii { get; set; }
        public DbSet<Categorie> Categorii { get; set; }
        public DbSet<Oras> Orase { get; set; }
        public DbSet<Serviciu> Servicii { get; set; }
        public DbSet<RaspunsRecenzie> Raspunsuri { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configurãm rela?iile (Cascade Delete la ?tergerea unei firme)
            modelBuilder.Entity<Recenzie>()
                .HasOne(r => r.Firma)
                .WithMany(f => f.Recenzii)
                .HasForeignKey(r => r.FirmaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
