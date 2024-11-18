using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace dermanovaPr.Models
{
    public class Trabajadores
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int TrabajadoresId { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Cedula { get; set; }

        [Required]
        public string Celular { get; set; }  // Cambiado a string para mayor flexibilidad


        [Required]
        public string Puesto { get; set; }
        // Llave foránea al ID de IdentityUser
        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]  // Vincula la propiedad de navegación User con UserId
        public IdentityUser User { get; set; }
        [Required]
        public bool State { get; set; }

        public ICollection<Citas> Citas { get; set; }= new List<Citas>();
    }
}
