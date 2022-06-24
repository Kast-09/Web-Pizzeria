using Maxdel.DB;
using Maxdel.Models;
using Microsoft.EntityFrameworkCore;

namespace Maxdel.Repositorio
{
    public interface IProcesarCompraRepositorio
    {
        Productos obtenerProducto(int IdProducto);
        void registrarPedido(int Id);
        int obtenerIdPedido();
        TamañoPrecio obtenerTamañoPrecio(int IdTamañoPrecio);
        void actualizarMonto(int idPedido, int Cantidad, decimal Precio);
        void agregarDetalle(DetallePedido detallePedido);
        List<DetallePedido> listarDatosCesta(int Id);
        List<Direcciones> obtenerDirecciones(int Id);
        List<DetallePedido> obtenerMontos(int Id);
        void actualizarCantidad(int Id, int Cantidad);
        Usuario obtenerUsuario(string username);
        Usuario obtenerUsuarioFirst(int Id);
        int obtenerSecuenciaTracking();
        void actualizarCodTracking();
        void eliminarPedido(int id);
        List<DetallePedido> listarPedidos(int Id);
        List<Pedido> listarPedidosActualizar(int Id);
        void comprar();
        int contarBoletas();
        Direcciones obtenerDireccion(int IdDireccion);
        int guardarBoleta(Boleta boleta);
    }
    public class ProcesarCompraRepositorio : IProcesarCompraRepositorio
    {
        private readonly DbEntities _dbEntities;

        public ProcesarCompraRepositorio(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }

        public Productos obtenerProducto(int IdProducto)
        {
            return _dbEntities.Productos
                                .Include("TamañoPrecios")
                                .First(o => o.Id == IdProducto);
        }

        public void registrarPedido(int Id)
        {
            Pedido pedido = new Pedido();
            pedido.IdUsuario = Id;
            pedido.Estado = 1;
            pedido.Destino = "Delivery";
            _dbEntities.pedidos.Add(pedido);
            _dbEntities.SaveChanges();
        }

        public int obtenerIdPedido()
        {
            return _dbEntities.pedidos.OrderBy(o => o.Id).Last().Id;
        }

        public TamañoPrecio obtenerTamañoPrecio(int IdTamañoPrecio)
        {
            return _dbEntities.tamañoPrecios.First(o => o.Id == IdTamañoPrecio);
        }

        public void actualizarMonto(int idPedido, int Cantidad, decimal Precio)
        {
            Pedido pedidoAux2 = new Pedido();
            pedidoAux2 = _dbEntities.pedidos.First(o => o.Id == idPedido);
            pedidoAux2.Monto = Cantidad * Precio;
            _dbEntities.SaveChanges();
        }

        public void agregarDetalle(DetallePedido detallePedido)
        {
            _dbEntities.detallePedidos.Add(detallePedido);
            _dbEntities.SaveChanges();
        }

        public List<DetallePedido> listarDatosCesta(int Id)
        {
            return _dbEntities.detallePedidos
                    .Include(o => o.Producto)
                    .Include(o => o.Pedido)
                    .Include(o => o.Pedido.EstadoFK)
                    .Where(o => o.Pedido.IdUsuario == Id && o.Pedido.Estado == 1).ToList();
        }

        public List<Direcciones> obtenerDirecciones(int Id)
        {
            return _dbEntities.direcciones
                    .Where(o => o.IdUsuario == Id).ToList();
        }

        public List<DetallePedido> obtenerMontos(int Id)
        {
            return _dbEntities.detallePedidos
                    .Include(o => o.Pedido)
                    .Where(o => o.Pedido.IdUsuario == Id && o.Pedido.Estado == 1).ToList();
        }

        public void actualizarCantidad(int Id, int Cantidad)
        {
            DetallePedido pedido = _dbEntities.detallePedidos.First(o => o.Id == Id);
            pedido.Cantidad = Cantidad;
            _dbEntities.SaveChanges();
        }

        public Usuario obtenerUsuario(string username)
        {
            return _dbEntities.usuarios.First(o => o.Correo == username);
        }

        public Usuario obtenerUsuarioFirst(int Id)
        {
            return _dbEntities.usuarios.First(o => o.Id == Id);
        }

        public int obtenerSecuenciaTracking()
        {
            return _dbEntities.nroTrackings.First(o => o.Id == 1).Numero;
        }

        public void actualizarCodTracking()
        {
            NroTracking nroTracking = new NroTracking();
            nroTracking = _dbEntities.nroTrackings.First(o => o.Id == 1);
            nroTracking.Numero++;
            _dbEntities.SaveChanges();
        }

        public void eliminarPedido(int id)
        {
            var pedidoDb = _dbEntities.pedidos.First(o => o.Id == id);
            _dbEntities.pedidos.Remove(pedidoDb);
            _dbEntities.SaveChanges();
            var detalleDb = _dbEntities.detallePedidos.First(o => o.IdPedido == id);
            _dbEntities.detallePedidos.Remove(detalleDb);
            _dbEntities.SaveChanges();
        }

        public List<DetallePedido> listarPedidos(int Id)
        {
            return _dbEntities.detallePedidos
                        .Include(o => o.Producto)
                        .Include(o => o.Pedido)
                        .Include(o => o.Pedido.EstadoFK)
                        .Where(o => o.Pedido.IdUsuario == Id && o.Pedido.Estado == 1).ToList();
        }

        public List<Pedido> listarPedidosActualizar(int Id)
        {
            return _dbEntities.pedidos.Where(o => o.IdUsuario == Id && o.Estado == 1).ToList();
        }

        public void comprar()
        {
            _dbEntities.SaveChanges();
        }

        public int contarBoletas()
        {
            return _dbEntities.boletas.Count();
        }

        public Direcciones obtenerDireccion(int IdDireccion)
        {
            return _dbEntities.direcciones.First(o => o.Id == IdDireccion);
        }

        public int guardarBoleta(Boleta boleta)
        {
            _dbEntities.boletas.Add(boleta);
            _dbEntities.SaveChanges();

            return _dbEntities.boletas.OrderBy(o => o.Id).Last().Id;
        }
    }
}
