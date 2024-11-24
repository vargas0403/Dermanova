using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace dermanovaPr.Models.Dtos
{
    public class DiagnosticoDTOS
    {
        public int DiagnosticosId { get; set; }
     
        public string Observaciobes { get; set; }

     
        public string Padecimientos { get; set; }
      
        public bool State { get; set; }
    }
}
