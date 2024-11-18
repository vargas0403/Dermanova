using dermanovaPr.Models.Dtos;
using dermanovaPr.Models.Responses;

namespace dermanovaPr.Services.InterfaceServices
{
    public interface IClientes
    {
        public Task<GetResponses> GetClientes();
        public Task<BaseResponses> AddClientes(ClientesDTOS clientesDtos);
        public Task<GetResponses> CedulaExist(string Cedula);
        public Task<GetResponses> CelularExist(string Celular);
    }
}
