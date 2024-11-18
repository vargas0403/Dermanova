using Azure;
using dermanovaPr.Data;
using dermanovaPr.Models.Responses;
using dermanovaPr.Services.InterfaceServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace dermanovaPr.Services
{
    public class UsersServices : IuserServices
    {

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IDbContextFactory<DataContext> _dbContextFactory;



        public UsersServices(IHttpContextAccessor contextAccessor,
            UserManager<IdentityUser> userManager, IDbContextFactory<DataContext> dbContextFactory,
            RoleManager<IdentityRole> roleManager)
        {

            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _dbContextFactory = dbContextFactory;
            _roleManager = roleManager;
        }
        public async Task<IdentityResult> ChangeUserPasswordAsync(string userId, string currentPassword, string newPassword)
        {
            
         var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("Usuario no encontrado.");
            }
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task<IdentityResult> DeleteUsers(string userId)
        {
            try
            {
                var exist = await _userManager.FindByIdAsync(userId);
                if (exist == null)
                {
                    Console.WriteLine("Usuario no encontrado");
                    return IdentityResult.Failed(new IdentityError
                    {
                        Description = "Usuario no encontrado"
                    });
                }
                var isadmin = await _userManager.IsInRoleAsync(exist, "Administrator");
                if (isadmin)
                {
                    Console.WriteLine("No se puede eliminar al usuario administrador");
                    return IdentityResult.Failed(new IdentityError
                    {
                        Description = "No se puede eliminar al usuario administrador"
                    });
                }
                return await _userManager.DeleteAsync(exist);

            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción inesperada
                Console.WriteLine($"Error al eliminar el usuario: {ex.Message}");
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Se produjo un error al intentar eliminar el usuario"
                });
            }

        }

        public async Task<IdentityResult> ForceChangePasswrd0(string UserId, string NewPassword)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(UserId);
                if (user == null)
                {
                    // Usuario no encontrado, devolver un resultado de error
                    return IdentityResult.Failed(new IdentityError
                    {
                        Description = "El usuario no existe"
                    });
                }
                else
                {
                    // Eliminar la contraseña actual
                    var removePasswordResult = await _userManager.RemovePasswordAsync(user);

                    if (!removePasswordResult.Succeeded)
                    {
                        // Si no se pudo eliminar la contraseña, devolver el resultado de error
                        return removePasswordResult;
                    }

                    // Agregar la nueva contraseña
                    var addPasswordResult = await _userManager.AddPasswordAsync(user, NewPassword);
                    return addPasswordResult;
                }
            }
            catch(Exception ex)
            {
                // Manejar cualquier excepción inesperada
                Console.WriteLine($"Error al eliminar el usuario: {ex.Message}");
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Se produjo un error al intentar eliminar el usuario"
                });  
            }
        }

        public async Task<string> GetTrabajadoresName()
        {
           var UserId= _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (UserId != null)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == UserId);
                return user?.UserName;
            }
            return null;
        }

        public async Task<GetResponses> GetUser(string UserId)
        {
            var response = new GetResponses();
            try
            {
                using (var context = _dbContextFactory.CreateDbContext())
                {
                    var trabajador = await context.Trabajadores
                        .Include(c => c.UserId).FirstOrDefaultAsync(c => c.UserId == UserId);
                    response.StatusCode = 200;
                    response.Message = "Success";
                    response.Tjs = trabajador;

                    

                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = "Error Getting CustomerData" + ex.Message;
            }

            return response;
        }

        public async Task<List<IdentityUser>> ObtenerTodosLosUsuariosAsync()
        {
            try
            {
                var result = await _userManager.Users.ToListAsync();
                if (result == null || !result.Any())
                {
                    // Opcionalmente puedes registrar un mensaje de advertencia si no hay usuarios
                    Console.WriteLine("No se encontraron usuarios en la base de datos.");
                    return new List<IdentityUser>(); // Retornar una lista vacía para evitar problemas
                }
                return result;
            }
            catch (Exception ex)
            {
                // Registramos el error con ILogger o Console.WriteLine
                Console.WriteLine($"Error en la obtención de datos: {ex.Message}");
                // En caso de error, retornar una lista vacía para evitar excepciones en la llamada
                return new List<IdentityUser>();
            }
        }

        public async Task<IdentityResult> UpdateUserName(string UserId, string NewUserName)
        {
             var user = await _userManager.FindByIdAsync(UserId);
                if(user == null)
                {

                    // Usuario no encontrado
                    return IdentityResult.Failed(new IdentityError
                    {
                        Description = "Usuario no encontrado"
                    });
                }


                // Verificar si el nuevo nombre de usuario ya está en uso por otro usuario
                var userWithNewName = await _userManager.FindByNameAsync(NewUserName);
                if (userWithNewName != null && userWithNewName.Id != user.Id)
                {
                    // El nuevo nombre de usuario ya está en uso
                    return IdentityResult.Failed(new IdentityError
                    {
                        Description = "El nombre de usuario ya está en uso"
                    });
                }

                // Cambiar el nombre de usuario
                user.UserName = NewUserName;

                // Intentar actualizar el usuario
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return IdentityResult.Success;
                }

                // Si la actualización falla, devolver los errores
                return result;


           
        }


        public async Task<IdentityResult> AddRole(string userId, string roleName)
        {

            try
            {
                // Busca al usuario por ID de forma asincrónica
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    // Si el usuario no existe, retorna un error
                    return IdentityResult.Failed(new IdentityError { Description = "User not found." });
                }

                // Verifica si el rol existe en la base de datos
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    // Si el rol no existe, crea el rol
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!roleResult.Succeeded)
                    {
                        return roleResult; // Devuelve el error si no se pudo crear el rol
                    }
                }

                // Asigna el rol al usuario
                var result = await _userManager.AddToRoleAsync(user, roleName);
                return result; // Devuelve el resultado de la asignación
            }
            catch (Exception ex)
            {
                // Registra o maneja la excepción según sea necesario
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }

        public async Task<List<IdentityRole>> GetRoles()
        {
            try
            {
                // Verificar si _roleManager está disponible
                if (_roleManager == null)
                {
                    Console.WriteLine("Error: _roleManager no está inicializado.");
                    return new List<IdentityRole>(); // Retorna lista vacía si no está inicializado
                }

                // Obtener los roles disponibles y convertirlos en una lista
                var roles = await _roleManager.Roles.ToListAsync();

                // Retorna los roles obtenidos o una lista vacía si no existen
                return roles ?? new List<IdentityRole>();
            }
            catch (Exception ex)
            {
                // Registrar el error detallado en la consola o en el log
                Console.WriteLine($"Error al obtener roles: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return new List<IdentityRole>(); // Retorna lista vacía si ocurre un error
            }
        }
    }
}
