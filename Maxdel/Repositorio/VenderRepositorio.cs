using Maxdel.DB;
using Maxdel.Models;
using Maxdel.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Maxdel.Repositorio
{
    public interface IVenderRepositorio
    {
        public List<Productos> obtenerProductos();
        Productos obtenerProducto(int IdProducto);
        int obtenerUltimoId();
        TamañoPrecio obtenerDetalle(int IdTamañoPrecio);
        void agregarUsuario(ClienteClaseIntermedia cliente);
        int obtenerUltimoIdUser();
        void agregarDatosBd(List<Pedido> Pedidos, List<DetallePedido> DetallePedidos);
        int contarBoletas();
        void agregarBoleta(Boleta boleta);
        int obtenerUltimoIdBoleta();
        Usuario obtenerUsuario(string username);
    }
    public class VenderRepositorio : IVenderRepositorio
    {
        private readonly DbEntities _dbEntities;

        public VenderRepositorio(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }

        public List<Productos> obtenerProductos()
        {
            return _dbEntities.Productos
                                        .Include("TamañoPrecios")
                                        .ToList();
        }

        public Productos obtenerProducto(int IdProducto)
        {
            return _dbEntities.Productos.FirstOrDefault(o => o.Id == IdProducto);
        }

        public int obtenerUltimoId()
        {
            return _dbEntities.pedidos.OrderBy(o => o.Id).Last().Id;
        }

        public TamañoPrecio obtenerDetalle(int IdTamañoPrecio)
        {
            return _dbEntities.tamañoPrecios.First(o => o.Id == IdTamañoPrecio);
        }

        public void agregarUsuario(ClienteClaseIntermedia cliente)
        {
            Usuario usuario = new Usuario();
            usuario.IdRol = 3;
            usuario.Nombre = cliente.Nombre;
            usuario.Apellido = cliente.Apellido;
            usuario.DNI = cliente.DNI;

            _dbEntities.usuarios.Add(usuario);
            _dbEntities.SaveChanges();
        }

        public int obtenerUltimoIdUser()
        {
            return _dbEntities.usuarios.OrderBy(o => o.Id).Last().Id;
        }

        public void agregarDatosBd(List<Pedido> Pedidos, List<DetallePedido> DetallePedidos)
        {
            _dbEntities.pedidos.AddRange(Pedidos);
            _dbEntities.detallePedidos.AddRange(DetallePedidos);
            _dbEntities.SaveChanges();
        }

        public int contarBoletas()
        {
            return _dbEntities.boletas.Count();
        }

        public void agregarBoleta(Boleta boleta)
        {
            _dbEntities.boletas.Add(boleta);
            _dbEntities.SaveChanges();
        }

        public int obtenerUltimoIdBoleta()
        {
            return _dbEntities.boletas.OrderBy(o => o.Id).Last().Id;
        }

        public Usuario obtenerUsuario(string username)
        {
            return _dbEntities.usuarios.First(o => o.Correo == username);
        }
    }
}
