using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using promociones.Data;
using promociones.Interfaces;
using promociones.Services;

namespace promociones.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoSyncService _sync;
        private readonly IProductoRepository _repo;

        public ProductoController(ProductoSyncService sync, IProductoRepository repo)
        {
            _sync = sync;
            _repo = repo;
        }

        // Dispara sincronización masiva desde /api/productos/listado
        [HttpPost("sync-listado")]
        public async Task<IActionResult> SyncListado()
        {
            try
            {
                await _sync.SyncListadoAsync();
                return Ok(new { mensaje = "Listado sincronizado" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // Sincroniza detalle de un producto concreto
        [HttpPost("{id}/sync-detail")]
        public async Task<IActionResult> SyncDetail(int id)
        {
            try
            {
                await _sync.SyncDetailIfNeededAsync(id);
                return Ok(new { mensaje = "Detalle sincronizado" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
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
        public async Task<IActionResult> GetProductosConPromocion([FromServices] AppDBContext db)
        {
            var q = from p in db.Productos
                    join pr in db.Promociones on p.IdPromocion equals pr.Id
                    where p.IdPromocion != null
                    select new
                    {
                        Producto = new { p.Id, p.Nombre, p.Descripcion, p.TienePromocion },
                        Promocion = new { pr.Id, pr.Descripcion, pr.Monto, pr.Porcentaje, pr.Fecha_inicio, pr.Fecha_limite }
                    };

            return Ok(await q.ToListAsync());
        }
    }
}