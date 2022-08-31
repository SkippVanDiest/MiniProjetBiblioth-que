using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniProjetBibliotheque.Models
{
    public class Livre
    {
        [Key]
        public int LivreId { get; set; }
        [Required]
        public string Titre { get; set; }
        public int NombrePages { get; set; }
        public int AuteurId { get; set; }
        public int DomaineId { get; set; }
        public Auteur Auteur { get; set; }
        public Domaine Domaine { get; set; }

        public virtual ICollection<Emprunt> EmpruntsLivre { get; set; }
    }
}
