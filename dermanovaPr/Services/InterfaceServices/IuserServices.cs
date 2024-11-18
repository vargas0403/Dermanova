using dermanovaPr.Models.Responses;
using Microsoft.AspNetCore.Identity;

namespace dermanovaPr.Services.InterfaceServices
{
    public interface IuserServices
    {
        Task<GetResponses> GetUser(string UserId);
        Task <string> GetTrabajadoresName();
        Task<List<IdentityUser>> ObtenerTodosLosUsuariosAsync();
        Task<IdentityResult> ChangeUserPasswordAsync(string userId, string currentPassword, string newPassword);
        Task<IdentityResult> DeleteUsers(string userId);
        Task<IdentityResult> ForceChangePasswrd0(string UserId, string NewPassword);
        Task<IdentityResult> UpdateUserName(string UserId, string NewUserName);
        Task<IdentityResult> AddRole(string userId, string roleName);
        Task<List<IdentityRole>> GetRoles();
    }
}
