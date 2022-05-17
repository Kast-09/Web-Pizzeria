using Maxdel.Models;
using Maxdel.DB;
using Microsoft.EntityFrameworkCore;

namespace Maxdel.Repositorio
{
    public interface IHomeRepositorio
    {
        List<DetallePedido> ObtenerDetalleCesta(int id);
        List<Producto> ObtenerProductos();
    }
    public class HomeRepositorio : IHomeRepositorio
    {
        private DbEntities _dbEntities;

        public HomeRepositorio(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }

        public List<DetallePedido> ObtenerDetalleCesta(int id)
        {
            return _dbEntities.detallePedidos
                    .Include(o => o.Producto)
                    .Include(o => o.Tamaño)
                    .Include(o => o.Precio)
                    .Include(o => o.Pedido)
                    .Where(o => o.IdPedido == id).ToList();
        }

        public List<Producto> ObtenerProductos()
        {
            return _dbEntities.Productos.ToList();
        }
    }
}
