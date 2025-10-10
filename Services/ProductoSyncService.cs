using System.Net.Http.Json;
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

        // Sincroniza todos los productos desde /api/productos (endpoint que incluye idPromocion)
        public async Task SyncListadoAsync()
        {
            var baseUrl = (_config.GetValue<string>("ProductosService:BaseUrl") ?? "").TrimEnd('/');
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new InvalidOperationException("ProductosService:BaseUrl no está configurado.");

            var url = $"{baseUrl}/api/productos";
            var lista = await _http.GetFromJsonAsync<List<ProductoDto>>(url);
            if (lista == null) return;

            // ids remotos y locales
            var idsRemotos = lista.Select(x => x.Id).ToHashSet();
            var idsLocales = await _repo.GetAllIdsAsync();
            var idsLocalesSet = idsLocales.ToHashSet();

            // Upsert por cada producto recibido (aquí traemos IdPromocion si existe)
            foreach (var item in lista)
            {
                var replica = new Producto
                {
                    Id = item.Id,
                    Nombre = item.Nombre ?? string.Empty,
                    // si el catálogo devuelve descripcion la guardamos, si no, no sobreescribimos en repo
                    Descripcion = item.Descripcion,
                    IdPromocion = item.IdPromocion,
                    TienePromocion = item.IdPromocion != null
                };

                await _repo.CreateOrUpdateAsync(replica);
            }

            // Eliminar locales que ya no existen en el catálogo
            var idsToDelete = idsLocalesSet.Except(idsRemotos).ToList();
            if (idsToDelete.Any())
            {
                await _repo.DeleteRangeAsync(idsToDelete);
            }
        }

        // Mantén tu SyncDetailIfNeededAsync si quieres llamadas puntuales por id (opcional)
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
                IdPromocion = detalle.IdPromocion,
                TienePromocion = detalle.IdPromocion != null
            };

            await _repo.CreateOrUpdateAsync(replica);
        }
    }
}
