using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dermanovaPr.Models
{
    public class Facturaciones
    {
        [Key/*, ForeignKey("Citas")*/] // Clave foránea y primaria
        public int FacturaId { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        // Relación uno a uno con Citas
        public Citas Citas { get; set; } // Propiedad de navegación

        [Required]
        public decimal Total { get; set; } // Total como propiedad regular

        [Required]
        public bool State { get; set; }

        public List<DetalleFactura>? Detalles { get; set; } = new List<DetalleFactura>();

        // Método para calcular el total
        public void CalcularTotal()
        {
            Total = Detalles.Sum(d => d.Subtotal);
        }

    }
}
