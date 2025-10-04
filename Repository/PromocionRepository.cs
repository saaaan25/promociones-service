using Microsoft.EntityFrameworkCore;
using promociones.Data;
using promociones.Interfaces;
using promociones.Models;

namespace promociones.Repository
{
    public class PromocionRepository : IPromocionRepository
    {
        private readonly AppDBContext _context;
        public PromocionRepository(AppDBContext context)
        {
            _context = context;
        }

        public Task<List<Promocion>> GetAllAsync()
        {
            return _context.Promociones.ToListAsync();
        }

    }
}