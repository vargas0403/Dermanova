using dermanovaPr.Data;
using dermanovaPr.Models;
using dermanovaPr.Models.Dtos;
using dermanovaPr.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using static dermanovaPr.Areas.Identity.Pages.Account.LoginModel;

namespace dermanovaPr.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
       
       private readonly DataContext _dataContext;
        public RegisterModel(SignInManager<IdentityUser> signInManager, DataContext dataContext, RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager)
      {
            _signInManager = signInManager;
            _userManager = userManager;
            _dataContext = dataContext;
            _roleManager = roleManager;
          
        }
        [BindProperty]
        public InputModel?
        Input
        {
            get;
            set;
        }
        [BindProperty]
        public Trabajadores TrabajadoreInput { get; set; }
       public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(Input.ConfirmPassword != Input.Password) 
            {
                ModelState.AddModelError(string.Empty, "la contraseño no coinciden");
                return Page();
            }
            else { 

            if (TrabajadoreInput == null)
            {
                ModelState.AddModelError(string.Empty, "Los datos de trabajadores no están completos.");
                return Page();
            }



                if (!ModelState.IsValid)
                {
                    using var transaction = await _dataContext.Database.BeginTransactionAsync();

                    var identity = new IdentityUser
                    {
                       // Email = Input.Email,
                        UserName = Input.UserName,

                    };
                 //   var result = await _userManager.CreateAsync(identity, Input.Password);
                    var exist = await _userManager.FindByNameAsync(Input.UserName);
                   // var existE = await _userManager.FindByEmailAsync(Input.Email);
                 

                    if (exist != null)
                    {
                        ModelState.AddModelError(string.Empty, "el nombre ya esta siendo utilizado");
                    }
                    //else if (existE != null)
                    //{
                    //    ModelState.AddModelError(string.Empty, "el Email ya esta siendo utilizado ");
                    //}
                    else
                    {
                        var result = await _userManager.CreateAsync(identity, Input.Password);

                        if (result.Succeeded)
                        {
                            if (_dataContext.Users.Count() == 1)
                            {
                                await _userManager.AddToRoleAsync(identity, "Administrador");

                            }
                            else
                            {

                                await _userManager.AddToRoleAsync(identity, "Secretaria");
                            };


                            var trabajadores = new Trabajadores
                            {
                                Nombre = TrabajadoreInput.Nombre,
                                Cedula = TrabajadoreInput.Cedula,
                                UserId = identity.Id,
                                State = true,
                                Celular = TrabajadoreInput.Celular,
                                Puesto = TrabajadoreInput.Puesto

                            };


                            _dataContext.Trabajadores.Add(trabajadores);
                            await _dataContext.SaveChangesAsync();
                            await _userManager.AddClaimAsync(identity, new Claim("TrabajadorId", trabajadores.TrabajadoresId.ToString()));
                            await transaction.CommitAsync();
                            //   await _signInManager.SignInAsync(identity, isPersistent: false);

                            // Si la creación del usuario falla, mostrar errores
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                            return LocalRedirect("/");
                        }
                    }
                }
            }
            return Page();
        }
    }
}
