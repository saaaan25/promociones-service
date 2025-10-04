using promociones.Models;

namespace promociones.Interfaces
{
    public interface IPromocionRepository
    {
        Task<List<Promocion>> GetAllAsync();
    }
}