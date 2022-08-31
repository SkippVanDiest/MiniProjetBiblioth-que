using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MiniProjetBibliotheque.Models
{
    public class Lecteur : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string NomLecteur { get; set; }
        [StringLength(50)]
        public string PrenomLecteur { get; set; }
        public string Adresse { get; set; }

        public virtual ICollection<Emprunt> EmpruntsLecteur { get; set; }
    }
}
