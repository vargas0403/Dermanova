using dermanovaPr.Models.Dtos;
using dermanovaPr.Models.Responses;

namespace dermanovaPr.Services.InterfaceServices
{
    public interface IPrestacionesServices
    {
        Task<BaseResponses> AddPrestaciones(PrestacionesDTOS dTOS);
        Task<GetResponses> GetPrestaciones();

    }
}
