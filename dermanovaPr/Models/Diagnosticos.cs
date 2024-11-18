using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace dermanovaPr.Models
{
    public class Diagnosticos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DiagnosticosId { get; set; }
        [Required]
        public string Observaciobes { get; set; }

        [Required]
        public string Padecimientos { get; set; }
        [Required]
        public bool State {  get; set; }
    }
}
