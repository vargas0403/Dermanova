using Microsoft.AspNetCore.Identity;

namespace dermanovaPr.Data.servicesR
{
    public class RolAdmins
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolAdmins( RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> CreateRole(string roleName)
        {
            // Verificar si el rol ya existe
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                // Crear el rol
                var role = new IdentityRole(roleName);
                var result = await _roleManager.CreateAsync(role);
                return result.Succeeded;
            }
            return false; // Devuelve falso si el rol ya existe
        }

        public async Task CreateRoles(params string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                await CreateRole(roleName); // Llama al método para crear cada rol
            }
        }
    }
}
