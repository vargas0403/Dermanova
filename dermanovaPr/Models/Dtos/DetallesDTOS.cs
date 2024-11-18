using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace dermanovaPr.Models.Dtos
{
    public class DetallesDTOS
    {
      

        public int DetalleFacturaId { get; set; }

       
        public int FacturacionesId { get; set; }

   

        public int PrestacionesId { get; set; }
  

   
        public int Cantidad { get; set; }

        
        public int Precio { get; set; }

      
        public int Subtotal {  get; set; }

        public bool State { get; set; }

    }
}
