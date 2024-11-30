using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CRUDVENTAS.Models;

namespace CRUDVENTAS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CRUDVENTAS.Models.Cliente> Cliente { get; set; } = default!;
        public DbSet<CRUDVENTAS.Models.Venta> Venta { get; set; } = default!;
        public DbSet<CRUDVENTAS.Models.Producto> Producto { get; set; } = default!;
        public DbSet<CRUDVENTAS.Models.DetalleVenta> DetalleVenta { get; set; } = default!;
    }
}
