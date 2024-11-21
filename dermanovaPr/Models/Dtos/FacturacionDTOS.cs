using System.ComponentModel.DataAnnotations;

namespace dermanovaPr.Models.Dtos
{
    public class FacturacionDTOS
    {
        public int FacturaId { get; set; }

        public DateTime Fecha { get; set; }

        // Relación uno a uno con Citas
        public Citas Citas { get; set; } // Propiedad de navegación

        public decimal Total { get; set; } // Total como propiedad regular

        public List<DetalleFactura>? Detalles { get; set; } = new List<DetalleFactura>();
        public List<DetallesDTOS> DetallesDTOS { get; set; } = new List<DetallesDTOS>();

        public bool State { get; set; }

    
        // Método para calcular el total
   
    }
}
