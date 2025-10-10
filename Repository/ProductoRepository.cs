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
            if (existing == null)
            {
                // Insertar nuevo
                _context.Productos.Add(producto);
            }
            else
            {
                // Actualizar solo campos no nulos / relevantes
                existing.Nombre = producto.Nombre ?? existing.Nombre;

                if (producto.Descripcion != null)
                    existing.Descripcion = producto.Descripcion;

                // Si llega IdPromocion explícitamente (aunque sea null?) consideramos HasValue.
                // Aquí actualizamos IdPromocion solo si viene un valor (HasValue = true).
                if (producto.IdPromocion.HasValue)
                {
                    existing.IdPromocion = producto.IdPromocion;
                    existing.TienePromocion = producto.IdPromocion != null;
                }
                else
                {
                    // Si no hay nuevo IdPromocion, conservamos el valor corriente
                    existing.TienePromocion = existing.IdPromocion != null;
                }

                _context.Entry(existing).State = EntityState.Modified;
            }

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

        public async Task DeleteRangeAsync(IEnumerable<int> ids)
        {
            var items = await _context.Productos.Where(p => ids.Contains(p.Id)).ToListAsync();
            if (!items.Any()) return;
            _context.Productos.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Producto>> GetAllAsync() => await _context.Productos.ToListAsync();

        public async Task<List<int>> GetAllIdsAsync()
        {
            return await _context.Productos.AsNoTracking().Select(p => p.Id).ToListAsync();
        }

        public async Task<Producto?> GetByIdAsync(int id) => await _context.Productos.FindAsync(id);
    }
}