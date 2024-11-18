
using dermanovaPr.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using dermanovaPr.Data.servicesR;
using dermanovaPr.Services.InterfaceServices;
using dermanovaPr.Services;
using dermanovaPr.Data.servicesR;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<RolAdmins>();
builder.Services.AddScoped<ITrabajadores,TrabajadoresServices>();
builder.Services.AddScoped<IClientes, ClientesServices>();
builder.Services.AddScoped<IuserServices, UsersServices>();
builder.Services.AddScoped<IPadecimientoServices, PadecimientosServices>();
builder.Services.AddScoped<IPrestacionesServices, PrestacionesServices>();
builder.Services.AddScoped<IregaliasServices, RelagaliasServices>();
builder.Services.AddScoped<Icitas_FacturacionServices, Citas_FacturacionServices>();
// esta es la variable que se utiliza para la conexion de la base de datos 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration= true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

//Registramos el DbContex
//Este es el enfoque recoemndado para blazor ya que se crean instancias segun se necesiten 
builder.Services.AddDbContextFactory<DataContext>(options => options.UseSqlServer(connectionString));
builder.Services
       .AddIdentity<IdentityUser, IdentityRole>(
        options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 5;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.SignIn.RequireConfirmedEmail = false;
        })
       .AddEntityFrameworkStores<DataContext>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    //Esta es vitas y crea las migraciones en el caso de que el sistema ocupe agregar nuevas actualizaciones
    //Esto acelera el proceso de creacion de datos...
    var DbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    DbContext.Database.EnsureCreated();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var Roles = new RolAdmins(roleManager); // Crea una instancia de ClassRoles

    // Llama al método para crear roles
    await Roles.CreateRoles("Administrador", "Secretaria", "Operador");
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
