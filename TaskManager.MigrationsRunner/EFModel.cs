using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster
{
    public class Utilisateur
    {
        public int IdUtilisateur { get; set; }
        public required string Nom { get; set; }
        public required string Prenom { get; set; }
        public required string Email { get; set; }
        public required string MotDePasse { get; set; }

        public ICollection<Tache> TachesAuteurs { get; set; } = new List<Tache>();
        public ICollection<Tache> TachesRealisateurs { get; set; } = new List<Tache>();
        public ICollection<Commentaire> Commentaires { get; set; } = new List<Commentaire>();
        public ICollection<HistoriqueModification> Modifications { get; set; } = new List<HistoriqueModification>();
    }
    public class Tache
    {
        public int IdTache { get; set; }
        public required string Titre { get; set; }
        public required string Description { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime Echeance { get; set; }
        public required string Statut { get; set; }
        public required string Priorite { get; set; }
        public int IdAuteur { get; set; }
        public required Utilisateur Auteur { get; set; }
        public int IdRealisateur { get; set; }
        public required Utilisateur Realisateur { get; set; }
        public required string Categorie { get; set; }

        public ICollection<SousTache> SousTaches { get; set; } = new List<SousTache>();
        public ICollection<TacheEtiquette> TachesEtiquettes { get; set; } = new List<TacheEtiquette>();
        public ICollection<Commentaire> Commentaires { get; set; } = new List<Commentaire>();
        public ICollection<HistoriqueModification> HistoriqueModifications { get; set; } = new List<HistoriqueModification>();
        public ICollection<TacheProjet> TachesProjets { get; set; } = new List<TacheProjet>();
    }
    public class SousTache
    {
        public int IdSousTache { get; set; }
        public int IdTacheParent { get; set; }
        public required Tache TacheParent { get; set; }
        public required string Titre { get; set; }
        public required string Statut { get; set; }
        public DateTime Echeance { get; set; }
    }
    public class Etiquette
    {
        public int IdEtiquette { get; set; }
        public required string Libelle { get; set; }

        public ICollection<TacheEtiquette> TachesEtiquettes { get; set; } = new List<TacheEtiquette>();
    }
    public class TacheEtiquette
    {
        public int IdTache { get; set; }
        public required Tache Tache { get; set; }

        public int IdEtiquette { get; set; }
        public required Etiquette Etiquette { get; set; }
    }
    public class Commentaire
    {
        public int IdCommentaire { get; set; }
        public int IdTache { get; set; }
        public required Tache Tache { get; set; }
        public int IdAuteur { get; set; }
        public required Utilisateur Auteur { get; set; }
        public DateTime DateCommentaire { get; set; }
        public required string Contenu { get; set; }
    }
    public class HistoriqueModification
    {
        public int IdModification { get; set; }
        public int IdTache { get; set; }
        public required Tache Tache { get; set; }
        public int IdAuteur { get; set; }
        public required Utilisateur Auteur { get; set; }
        public DateTime DateModification { get; set; }
        public required string DescriptionModif { get; set; }
    }
    public class Projet
    {
        public int IdProjet { get; set; }
        public required string NomProjet { get; set; }
        public required string DescriptionProjet { get; set; }

        public ICollection<TacheProjet> TachesProjets { get; set; } = new List<TacheProjet>();
    }
    public class TacheProjet
    {
        public int IdTache { get; set; }
        public required Tache Tache { get; set; }

        public int IdProjet { get; set; }
        public required Projet Projet { get; set; }
    }

}
