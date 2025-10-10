using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using promociones.Data;
using promociones.Dtos.Producto;
using promociones.Interfaces;
using promociones.Services;

namespace promociones.Controllers
{
    [ApiController]
    [Route("api/productos")]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoSyncService _sync;
        private readonly IProductoRepository _repo;

        public ProductoController(ProductoSyncService sync, IProductoRepository repo)
        {
            _sync = sync;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var all = await _repo.GetAllAsync();
            return Ok(all);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var producto = await _repo.GetByIdAsync(id);
            if (producto == null) return NotFound();
            return Ok(producto);
        }

        // Productos que tienen promoción
        [HttpGet("con-promocion")]
        public async Task<IActionResult> GetProductosConPromocion()
        {
            var productos = await _repo.GetAllAsync();

            var conPromocion = productos
                .Where(p => p.TienePromocion && p.IdPromocion != null)
                .Select(p => new
                {
                    producto = new
                    {
                        id = p.Id,
                        nombre = p.Nombre,
                        descripcion = p.Descripcion,
                        tienePromocion = true
                    },
                    idPromocion = p.IdPromocion
                })
                .ToList();

            return Ok(conPromocion);
        }
    }
}