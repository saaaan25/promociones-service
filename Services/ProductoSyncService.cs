using promociones.Dtos.Producto;
using promociones.Interfaces;
using promociones.Models;

namespace promociones.Services
{
    public class ProductoSyncService
    {
        private readonly HttpClient _http;
        private readonly IProductoRepository _repo;
        private readonly IConfiguration _config;

        public ProductoSyncService(HttpClient http, IProductoRepository repo, IConfiguration config)
        {
            _http = http;
            _repo = repo;
            _config = config;
        }

        // /api/productos/listado
        public async Task SyncListadoAsync()
        {
            var baseUrl = (_config.GetValue<string>("ProductosService:BaseUrl") ?? "").TrimEnd('/');
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new InvalidOperationException("ProductosService:BaseUrl no está configurado.");

            var url = $"{baseUrl}/api/productos/listado";
            var lista = await _http.GetFromJsonAsync<List<ProductoListadoDto>>(url);
            if (lista == null) return;

            foreach (var item in lista)
            {
                var replica = new Producto
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    TienePromocion = item.TienePromocion,
                };

                await _repo.CreateOrUpdateAsync(replica);
            }
        }

        // /api/productos/{id}
        public async Task SyncDetailIfNeededAsync(int productId)
        {
            var baseUrl = (_config.GetValue<string>("ProductosService:BaseUrl") ?? "").TrimEnd('/');
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new InvalidOperationException("ProductosService:BaseUrl no está configurado.");

            var url = $"{baseUrl}/api/productos/{productId}";
            var detalle = await _http.GetFromJsonAsync<ProductoDetalleDto>(url);
            if (detalle == null) return;

            var replica = new Producto
            {
                Id = detalle.Id,
                Nombre = detalle.Nombre,
                Descripcion = detalle.Descripcion,
                TienePromocion = detalle.IdPromocion != null,
                IdPromocion = detalle.IdPromocion
            };

            await _repo.CreateOrUpdateAsync(replica);
        }
    }
}