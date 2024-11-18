using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.ComponentModel.DataAnnotations;

namespace dermanovaPr.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public LoginModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(Input.UserName);
                if (user != null)
                {
                 

                    var result = await _signInManager.PasswordSignInAsync(user, Input.Password, isPersistent: false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return LocalRedirect("/");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Credenciales incorrectas. Por favor verifica tu correo y contrase�a.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Credenciales incorrectas. Por favor verifica tu correo y contrase�a.");
                }
            }

            // Si llegamos a este punto, algo fall�, volver a mostrar el formulario
            return Page();
        }
        public class InputModel
        {
            //[Required(ErrorMessage = "El correo electr�nico es obligatorio.")]
            //[EmailAddress(ErrorMessage = "El correo electr�nico no es v�lido.")]
            //public string Email { get; set; }

            [Required(ErrorMessage = "La contrase�a es obligatoria.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required(ErrorMessage = "La confirmaci�n de la contrase�a es obligatoria.")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Las contrase�as no coinciden.")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string UserName { get; set; }
        }
    }
}
