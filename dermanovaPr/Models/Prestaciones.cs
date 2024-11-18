using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dermanovaPr.Models
{
    public class Prestaciones
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PrestacionesId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int costo { get; set; }
        [Required]
        public bool State {  get; set; }

        public ICollection<Citas> Citas { get; set; }= new List<Citas>();
    }
}
