using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace dermanovaPr.Models
{
    public class DetalleFactura
    {
        
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int DetalleFacturaId { get; set; }

            
            public int? FacturacionesId { get; set; }

            public Facturaciones? Facturacione { get; set; }

            [Required]
            public int PrestacionesId { get; set; }
            public Prestaciones Prestacion { get; set; }

            [Required]
            public int Cantidad { get; set; }

            [Required]
          
            public int Precio { get; set; }

            [NotMapped]
            public int Subtotal => Cantidad * Precio;
            
            public bool State { get; set; }

    }

}
