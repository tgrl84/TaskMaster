using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TaskMaster
{
    public class TaskManagerDbContext : DbContext
    {
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Tache> Taches { get; set; }
        public DbSet<SousTache> SousTaches { get; set; }
        public DbSet<Etiquette> Etiquettes { get; set; }
        public DbSet<TacheEtiquette> TachesEtiquettes { get; set; }
        public DbSet<Commentaire> Commentaires { get; set; }
        public DbSet<HistoriqueModification> HistoriqueModifications { get; set; }
        public DbSet<Projet> Projets { get; set; }
        public DbSet<TacheProjet> TachesProjets { get; set; }

        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration de la clé primaire pour Utilisateur
            modelBuilder.Entity<Utilisateur>()
                .HasKey(u => u.IdUtilisateur);

            // Configuration de la clé primaire pour Tache
            modelBuilder.Entity<Tache>()
                .HasKey(t => t.IdTache);

            // Configuration de la clé primaire pour SousTache
            modelBuilder.Entity<SousTache>()
                .HasKey(st => st.IdSousTache);

            // Configuration de la clé primaire pour Etiquette
            modelBuilder.Entity<Etiquette>()
                .HasKey(e => e.IdEtiquette);

            // Configuration de la clé primaire pour TacheEtiquette
            modelBuilder.Entity<TacheEtiquette>()
                .HasKey(te => new { te.IdTache, te.IdEtiquette });

            // Configuration de la clé primaire pour Commentaire
            modelBuilder.Entity<Commentaire>()
                .HasKey(c => c.IdCommentaire);

            // Configuration de la clé primaire pour HistoriqueModification
            modelBuilder.Entity<HistoriqueModification>()
                .HasKey(hm => hm.IdModification);

            // Configuration de la clé primaire pour Projet
            modelBuilder.Entity<Projet>()
                .HasKey(p => p.IdProjet);

            // Configuration de la clé primaire pour TacheProjet
            modelBuilder.Entity<TacheProjet>()
                .HasKey(tp => new { tp.IdTache, tp.IdProjet });

            // Configuration des relations de clé étrangère
            modelBuilder.Entity<Tache>()
                .HasOne(t => t.Auteur)
                .WithMany(u => u.TachesAuteurs)
                .HasForeignKey(t => t.IdAuteur)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Tache>()
                .HasOne(t => t.Realisateur)
                .WithMany(u => u.TachesRealisateurs)
                .HasForeignKey(t => t.IdRealisateur)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
