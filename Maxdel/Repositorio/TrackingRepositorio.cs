using Maxdel.DB;
using Maxdel.Models;
using Microsoft.EntityFrameworkCore;

namespace Maxdel.Repositorio
{
    public interface ITrackingRepositorio
    {
        DetallePedido obtenerEstado(string CodTracking);
    }
    public class TrackingRepositorio: ITrackingRepositorio
    {
        private readonly DbEntities _dbEntities;

        public TrackingRepositorio(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }

        public DetallePedido obtenerEstado(string CodTracking)
        {
            return _dbEntities.detallePedidos
                                           .Include(o => o.Pedido)
                                           .Include(o => o.Producto)
                                           .FirstOrDefault(o => o.Pedido.CodTracking == CodTracking);
        }
    }
}
