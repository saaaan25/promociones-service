using promociones.Models;

namespace promociones.Interfaces
{
    public interface IProductoRepository
    {
        Task<List<Producto>> GetAllAsync();
        Task<Producto?> GetByIdAsync(int id);
        Task<List<int>> GetAllIdsAsync();
        Task<Producto> CreateOrUpdateAsync(Producto p);
        Task DeleteRangeAsync(IEnumerable<int> ids);
        Task<bool> DeleteAsync(int id);
    }
}
