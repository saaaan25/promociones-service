using Microsoft.EntityFrameworkCore;
using promociones.Data;
using promociones.Interfaces;
using promociones.Models;

namespace promociones.Repository
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly AppDBContext _context;
        public ProductoRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Producto> CreateOrUpdateAsync(Producto producto)
        {
            var existing = await _context.Productos.FirstOrDefaultAsync(x => x.Id == producto.Id);
            if (existing == null) _context.Productos.Add(producto);
            else _context.Entry(existing).CurrentValues.SetValues(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var e = await _context.Productos.FindAsync(id);
            if (e == null) return false;
            _context.Productos.Remove(e);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Producto>> GetAllAsync() => await _context.Productos.ToListAsync();

        public async Task<Producto?> GetByIdAsync(int id) => await _context.Productos.FindAsync(id);

    }
}