/*
 * DESCRIERE:
 * Contextul bazei de date a aplicației, extinzând IdentityDbContext pentru integrarea ASP.NET Core Identity.
 * Configurează seturile de date (DbSet) pentru toate modelele de business și definește relațiile (precum relația One-to-One dintre Firmă și Contact) prin ModelBuilder.
 */

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using recenzi_pentru_firme.Models;

namespace recenzi_pentru_firme.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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
        public DbSet<Contact> Contacte { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relație 1:1 între Firma și Contact
            modelBuilder.Entity<Firma>()
                .HasOne(f => f.Contact)
                .WithOne(c => c.Firma)
                .HasForeignKey<Contact>(c => c.FirmaId);
        }
    }
}
