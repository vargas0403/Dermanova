using dermanovaPr.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dermanovaPr.Data
{
    public class DataContext : IdentityDbContext
    {

        public DbSet<Trabajadores> Trabajadores { get; set; }
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Prestaciones> prestaciones { get; set; }
        public DbSet<Facturaciones> Facturaciones { get; set; }
        public DbSet<Diagnosticos> Diagnosticos { get; set; }
        public DbSet<Citas> Citas { get; set; }
        public DbSet<Regalias> regaliases { get; set; }
        public DbSet<Padecimientos> Padecimientos { get; set; }
        public DbSet<DetalleFactura> DetalleFacturas { get; set; }
        // se declaro un metodo constructor  exclusivo para el DBcontext de la base de datos  
        public DataContext(DbContextOptions options) : base(options)
        {
                
        }
    }
}
