using Microsoft.AspNetCore.Mvc;
using promociones.Data;
using promociones.Mappers;

namespace promociones.Controllers
{
    [Route("/api/promociones")]
    [ApiController]
    public class PromocionController : ControllerBase
    {
        private readonly AppDBContext _context;
        public PromocionController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var promociones = _context.Promociones.ToList().Select(p => p.ToPromocionDto());

            return Ok(promociones);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var promocion = _context.Promociones.Find(id);

            if (promocion == null)
            {
                return NotFound();
            }

            return Ok(promocion.ToPromocionDto());
        }
    }
}