using System.ComponentModel.DataAnnotations;

namespace MiniProjetBibliotheque.Models
{
    public class Auteur
    {
        [Key]
        public int AuteurId { get; set; }
        [Required]
        [StringLength(50)]
        public string NomAuteur { get; set; }
        [StringLength(50)]
        public string PrenomAuteur { get; set; }
        public string? EmailAuteur { get; set; }
        public int? TelephoneAuteur { get; set; }
        public string? Grade { get; set; }

        public virtual ICollection<Livre> LivresAuteur { get; set; }
    }
}
