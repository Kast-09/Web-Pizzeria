using Maxdel.DB;
using Maxdel.Models;
using Microsoft.EntityFrameworkCore;

namespace Maxdel.Repositorio
{
    public interface IPedidosRepositorio
    {
        List<Boleta> listarEspera();
        List<Boleta> listarEntregado();
        List<Boleta> listarAnulado();
        List<Pedido> listarPedidos(int id);
        Usuario obtenerUsuario(string username);
        void actualizarEstado();
    }
    public class PedidosRepositorio: IPedidosRepositorio
    {
        private readonly DbEntities _dbEntities;
        public PedidosRepositorio(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }
        public List<Boleta> listarEspera()
        {
            return _dbEntities.boletas
                                .Include("Pedidos")
                                .Include("Pedidos.EstadoFK")
                                .Include("Pedidos.DetallePedidos")
                                .Include("Pedidos.DetallePedidos.Producto")
                                .Where(o => o.Pedidos.Any(x => x.Estado >= 2) && o.Pedidos.Any(x => x.Estado <= 4)).ToList();
        }

        public List<Boleta> listarEntregado()
        {
            return _dbEntities.boletas
                                .Include("Pedidos")
                                .Include("Pedidos.EstadoFK")
                                .Include("Pedidos.DetallePedidos")
                                .Include("Pedidos.DetallePedidos.Producto")
                                .Where(o => o.Pedidos.Any(x => x.Estado == 5)).ToList();
        }

        public List<Boleta> listarAnulado()
        {
            return _dbEntities.boletas
                                .Include("Pedidos")
                                .Include("Pedidos.EstadoFK")
                                .Include("Pedidos.DetallePedidos")
                                .Include("Pedidos.DetallePedidos.Producto")
                                .Where(o => o.Pedidos.Any(x => x.Estado == 6)).ToList();
        }

        public List<Pedido> listarPedidos(int id)
        {
            return _dbEntities.pedidos.Where(o => o.IdBoleta == id).ToList();
        }

        public void actualizarEstado()
        {
            _dbEntities.SaveChanges();
        }

        public Usuario obtenerUsuario(string username)
        {
            return _dbEntities.usuarios.First(o => o.Correo == username);
        }
    }
}
