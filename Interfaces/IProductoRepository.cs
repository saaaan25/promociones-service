using promociones.Models;

namespace promociones.Interfaces
{
    public interface IProductoRepository
    {
        Task<List<Producto>> GetAllAsync();
        Task<Producto?> GetByIdAsync(int id);
        Task<Producto> CreateOrUpdateAsync(Producto producto);
        Task<bool> DeleteAsync(int id);
    }
}