using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dermanovaPr.Models
{
    public class Padecimientos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public int PadecimientosId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        
        public string Description { get; set; }
        [Required]
        public bool State {  get; set; }
        public ICollection<Citas> Citas { get; set; } = new List<Citas>();
    }
}
