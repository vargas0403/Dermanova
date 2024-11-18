using dermanovaPr.Models.Dtos;
using dermanovaPr.Models.Responses;

namespace dermanovaPr.Services.InterfaceServices
{
    public interface ITrabajadores
    {
        Task<BaseResponses> AddTrabajador(TrabajadoresDTOS trabajadores);
        Task<GetResponses> GetTrabajadores();
    }
}
