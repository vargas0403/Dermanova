using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace dermanovaPr.Models.Dtos
{
    public class PrestacionesDTOS
    {
        
        public int PrestacionesId { get; set; }
    
        public string Name { get; set; }
       
        public string Description { get; set; }
     
        public int costo { get; set; }
      
        public bool State { get; set; }
    }
}
