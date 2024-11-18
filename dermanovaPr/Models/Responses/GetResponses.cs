namespace dermanovaPr.Models.Responses
{
    public class GetResponses:BaseResponses
    {

        public bool IsSuccess { get; set; } = false;
        public object? Data { get; set; }
        public List<Clientes>? Listclientes { get; set; }
        public Clientes? client { get; set; }
        public List<Trabajadores>? trabjL { get; set; }
        public Trabajadores? Tjs { get; set; }
        public List<Regalias>? ListRegalias { get; set; }
        public Regalias? Regls { get; set; }
        public Facturaciones? Fact { get; set; }

        public List<Facturaciones>? ListFact { get; set; }
        public List<Prestaciones>? ListPres { get; set; }
        public Prestaciones Prts { get; set; }
        public int Contar { get; set; }

        public List<Padecimientos> ListPadecimientos { get; set; }
        public Padecimientos? Padecimientos { get; set; }

        public  Citas?  Cits { get; set; }
        public List<Citas>? ListCitas { get; set; }  
        public DetalleFactura? DFT { get; set; }

        public List<DetalleFactura>? ListDetalleFactura { get; set; }
    }
}
