namespace promociones.Dtos.Producto
{
    public class ProductImageDto
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public bool Principal { get; set; }
        public string Imagen { get; set; } = string.Empty;
    }
    
    public class ProductoDetalleDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int? IdPromocion { get; set; }
        public List<ProductImageDto>? ProductoImagenes { get; set; }
    }
}