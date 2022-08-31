using System.ComponentModel.DataAnnotations;

namespace MiniProjetBibliotheque.Models
{
    public class Emprunt
    {
        [Key]
        public int EmpruntId { get; set; }
        public string Id { get; set; } // LecteurId
        public int LivreId { get; set; }
        public DateTime DateEmprunt { get; set; }
        public DateTime DateRetour { get; set; }
        public Lecteur Lecteur { get; set; }
        public Livre Livre { get; set; }
    }
}
