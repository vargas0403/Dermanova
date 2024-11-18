using System.ComponentModel.DataAnnotations;

namespace dermanovaPr.Models.Dtos
{
    public class ClientesDTOS
    {


        public int ClienteId { get; set; }
       
        public string Nombre { get; set; }
        
        public string Cedula { get; set; }
        
        public string Celular { get; set; }

        public bool State { get; set; }
    }
}
