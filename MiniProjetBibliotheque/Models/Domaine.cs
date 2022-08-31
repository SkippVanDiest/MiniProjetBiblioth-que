using System.ComponentModel.DataAnnotations;

namespace MiniProjetBibliotheque.Models
{
    public class Domaine
    {
        [Key]
        public int DomaineId { get; set; }
        [Required]
        public string NomDomaine { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Livre> LivresDomaine { get; set; }

    }
}
