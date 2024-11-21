using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace dermanovaPr.Models
{


    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Citas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CitasId { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public  string tipo {get ; set ;}
        [Required]
        public TimeSpan Hora { get; set; }

        // Conexión con Clientes
        [Required]
        public int ClienteId { get; set; }
        public Clientes Cliente { get; set; }

        // Conexión con Trabajadores
        [Required]
        public int TrabajadorId { get; set; }
        public Trabajadores Trabajador { get; set; }

        // Conexión con Regalias (nullable)
        public int? RegaliaId { get; set; }
        public Regalias Regalia { get; set; }

        // Conexión con Diagnosticos (nullable)
        public int? DiagnosticoId { get; set; }
        public Diagnosticos Diagnostico { get; set; }

        // Conexión con Padecimientos (nullable)
        public int? PadecimientoId { get; set; }
        public Padecimientos Padecimiento { get; set; }

        // Relación uno a uno con Facturaciones
        //[ForeignKey("Facturaciones")]
        public int? FacturaId { get; set; }
        public Facturaciones? Factura { get; set; } // Propiedad de navegación

        [Required]
        public bool State { get; set; }
    }

}
