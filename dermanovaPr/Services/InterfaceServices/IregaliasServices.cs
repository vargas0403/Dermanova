using dermanovaPr.Models.Dtos;
using dermanovaPr.Models.Responses;

namespace dermanovaPr.Services.InterfaceServices
{
    public interface IregaliasServices
    {
        Task<BaseResponses> AddRegalias(RegaliasDTOS dTOS);
        Task<GetResponses> GetRegalias();
        Task<GetResponses> CountReg(int Id);
        Task<GetResponses> DeleteR(int Id);

    }
}
