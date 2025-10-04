using Microsoft.AspNetCore.Mvc;
using promociones.Data;

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
            var promociones = _context.Promociones.ToList();

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

            return Ok(promocion);
        }
    }
}