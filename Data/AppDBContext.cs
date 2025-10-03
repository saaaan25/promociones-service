using Microsoft.EntityFrameworkCore;
using promociones.Models;

namespace promociones.Data;

public class AppDBContext : DbContext
{

    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

    public DbSet<Promocion> Promociones { get; set; }

}