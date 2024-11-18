using dermanovaPr.Models.Dtos;
using dermanovaPr.Models.Responses;

namespace dermanovaPr.Services.InterfaceServices
{
    public interface Icitas_FacturacionServices
    {
        public  Task<BaseResponses> AddCitaFac(FacturacionDTOS FCdTOS, List<DetallesDTOS> DTdtosList, CitasDTOS CTdtos);

        public Task<GetResponses> GetCitas(int Id);

        public Task<GetResponses> GetCitaTotal();

        public Task<GetResponses> GetCitasAgrupadasPorFechaYHora();



    }
}
