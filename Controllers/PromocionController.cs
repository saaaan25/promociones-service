using Microsoft.AspNetCore.Mvc;
using promociones.Data;
using promociones.Dtos.Promocion;
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

        [HttpPost]
        public IActionResult Create([FromBody] CreatePromocionRequestDto promocionDto)
        {
            var promocion = promocionDto.ToPromocionFromCreateDto();

            _context.Promociones.Add(promocion);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = promocion.Id }, promocion.ToPromocionDto());
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdatePromocionRequestDto updateDto)
        {
            var promocion = _context.Promociones.FirstOrDefault(p => p.Id == id);

            if (promocion == null)
            {
                return NotFound();
            }

            promocion.Descripcion = updateDto.Descripcion;
            promocion.Monto = updateDto.Monto;
            promocion.Porcentaje = updateDto.Porcentaje;
            promocion.Fecha_inicio = updateDto.Fecha_inicio.ToUniversalTime();
            promocion.Fecha_limite = updateDto.Fecha_limite.ToUniversalTime();

            _context.SaveChanges();

            return Ok(promocion.ToPromocionDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var promocion = _context.Promociones.FirstOrDefault(p => p.Id == id);

            if (promocion == null)
            {
                return NotFound();
            }

            _context.Promociones.Remove(promocion);
            _context.SaveChanges();

            return NoContent();
        }
    }
}