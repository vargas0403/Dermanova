using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace dermanovaPr.Models
{
    public class Clientes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ClienteId { get; set; }
        [Required]
        public string  Nombre { get; set; }
        
        public DateTime? FechaNac {  get; set; }
        public string? Cedula { get; set; }
        [Required]
        public string Celular { get; set; }

        [Required]
        public bool State  { get; set; }

        public ICollection<Citas> Citas { get; set; }= new List<Citas>();
       
    }
}
