using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniProjetBibliotheque.Models;

namespace MiniProjetBibliotheque.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Lecteur> Lecteurs { get; set; }
        public DbSet<Auteur> Auteurs { get; set; }
        public DbSet<Domaine> Domaines { get; set; }
        public DbSet<Livre> Livres { get; set; }
        public DbSet<Emprunt> Emprunts { get; set; }
    }
}