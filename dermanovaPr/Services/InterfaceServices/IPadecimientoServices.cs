using dermanovaPr.Models.Responses;

namespace dermanovaPr.Services.InterfaceServices
{
    public interface IPadecimientoServices
    {

        Task<GetResponses> GetPadecimientosAsync();
        Task<BaseResponses> AddPadecimientos(PadecimientoDTOS padecimientoDTOS);

    }   
}
