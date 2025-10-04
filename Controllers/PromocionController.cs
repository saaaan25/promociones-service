using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using promociones.Data;
using promociones.Dtos.Promocion;
using promociones.Interfaces;
using promociones.Mappers;

namespace promociones.Controllers
{
    [Route("/api/promociones")]
    [ApiController]
    public class PromocionController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IPromocionRepository _promocionRepository;
        public PromocionController(AppDBContext context, IPromocionRepository promocionRepository)
        {
            _promocionRepository = promocionRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var promociones = await _promocionRepository.GetAllAsync();
            
            var promocionesResult = promociones.Select(p => p.ToPromocionDto());

            return Ok(promocionesResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var promocion = await _context.Promociones.FindAsync(id);

            if (promocion == null)
            {
                return NotFound();
            }

            return Ok(promocion.ToPromocionDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePromocionRequestDto insertDto)
        {
            var promocion = insertDto.ToPromocionFromCreateDto();

            await _context.Promociones.AddAsync(promocion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = promocion.Id }, promocion.ToPromocionDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePromocionRequestDto updateDto)
        {
            var promocion = await _context.Promociones.FirstOrDefaultAsync(p => p.Id == id);

            if (promocion == null)
            {
                return NotFound();
            }

            promocion.Descripcion = updateDto.Descripcion;
            promocion.Monto = updateDto.Monto;
            promocion.Porcentaje = updateDto.Porcentaje;
            promocion.Fecha_inicio = updateDto.Fecha_inicio.ToUniversalTime();
            promocion.Fecha_limite = updateDto.Fecha_limite.ToUniversalTime();

            await _context.SaveChangesAsync();

            return Ok(promocion.ToPromocionDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var promocion = await _context.Promociones.FirstOrDefaultAsync(p => p.Id == id);

            if (promocion == null)
            {
                return NotFound();
            }

            _context.Promociones.Remove(promocion);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}