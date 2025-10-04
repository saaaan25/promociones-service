using Npgsql.Replication;
using promociones.Dtos.Promocion;
using promociones.Models;

namespace promociones.Mappers
{
    public static class PromocionMappers
    {
        public static PromocionDto ToPromocionDto(this Promocion promocionModel)
        {
            return new PromocionDto
            {
                Id = promocionModel.Id,
                Descripcion = promocionModel.Descripcion,
                Monto = promocionModel.Monto,
                Porcentaje = promocionModel.Porcentaje,
                Fecha_inicio = promocionModel.Fecha_inicio,
                Fecha_limite = promocionModel.Fecha_limite
            };
        }
    }
}