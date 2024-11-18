using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace dermanovaPr.Models
{
    public class Regalias
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RegaliasId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Marcas {  get; set; }
        [Required]
        public int Unidades { get; set; }
        [Required]
        public bool State {  get; set; }
    
        public ICollection<Citas> Citas { get; set; } = new List<Citas>();
    }
}
