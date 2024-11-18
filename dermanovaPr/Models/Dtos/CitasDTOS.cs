using System.ComponentModel.DataAnnotations;

namespace dermanovaPr.Models.Dtos
{
    public class CitasDTOS
    {
            public int CitasId { get; set; }

        //    public DateTime Fecha { get; set; }

        //    public TimeSpan Hora { get; set; }
        //    // Conexión con Clientes

           public int ClienteId { get; set; } 
        //    // Conexión con rabajadores

            public int TrabajadorId { get; set; }

        //    // Conexión con Regalias (nullable)
           public int? RegaliaId { get; set; }
        //    // Conexión con Diagnosticos (nullable)
            public int? DiagnosticoId { get; set; }
        //    // Conexión con Padecimientos (nullable)

          public int? PadecimientoId { get; set; }
        //    // Relación uno a uno con Facturaciones 
            public int? FacturaId { get; set; }
        //    public bool State { get; set; }
        //    public List<DetallesDTOS> DetallesFactura { get; set; } = new();

      
           // public int CitasId { get; set; }
            public DateTime Fecha { get; set; }
            public TimeSpan Hora { get; set; }
           // public int ClienteId { get; set; }
            public string ClienteNombre { get; set; }
            public string padecimientoNombre { get; set; }
            
            public string TrabajadorNombre { get; set; }
            //public int TrabajadorId { get; set; }
           // public int PadecimientoId { get; set; }
            public decimal TotalFactura { get; set; }
            public List<DetallesDTOS> DetallesFactura { get; set; } = new();
        





    }
}

