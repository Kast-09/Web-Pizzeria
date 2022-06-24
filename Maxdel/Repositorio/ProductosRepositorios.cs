using Maxdel.DB;
using Maxdel.Models;
using Microsoft.EntityFrameworkCore;

namespace Maxdel.Repositorio
{
    public interface IProductosRepositorios
    {
        List<Productos> listarProductos();
        void agregarProducto(Productos productos);
        Productos obtenerProducto(int Id);
        void actualizarProducto(int Id, Productos productos);
        void AgregarTamañoPrecio(int Id, TamañoPrecio tamañoPrecio);
        TamañoPrecio obtenerTamañoPrecio(int Id);
        Productos obtenerProductoId(int IdProducto);
        void editarTamaño(int Id, string TamañoProducto, decimal Precio);
        void eliminarTamañoPrecio(int Id);
        Usuario obtenerUsuario(string username);
    }
    public class ProductosRepositorios : IProductosRepositorios
    {
        private readonly DbEntities _dbEntities;

        public ProductosRepositorios(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }

        public List<Productos> listarProductos()
        {
            return _dbEntities.Productos
                                        .Include("TamañoPrecios")
                                        .ToList();
        }

        public void agregarProducto(Productos productos)
        {
            _dbEntities.Productos.Add(productos);
            _dbEntities.SaveChanges();
        }

        public Productos obtenerProducto(int Id)
        {
            return _dbEntities.Productos.First(o => o.Id == Id);
        }

        public void actualizarProducto(int Id, Productos productos)
        {
            var producto = _dbEntities.Productos.First(o => o.Id == Id);
            producto.Nombre = productos.Nombre;
            producto.Descripcion = productos.Descripcion;
            producto.UrlImagen = productos.UrlImagen;
            _dbEntities.SaveChanges();
        }

        public void AgregarTamañoPrecio(int Id, TamañoPrecio tamañoPrecio)
        {
            TamañoPrecio tamañoPrecio1 = new TamañoPrecio();
            tamañoPrecio1.IdProducto = Id;
            tamañoPrecio1.TamañoProducto = tamañoPrecio.TamañoProducto;
            tamañoPrecio1.Precio = tamañoPrecio.Precio;
            _dbEntities.tamañoPrecios.Add(tamañoPrecio1);
            _dbEntities.SaveChanges();
        }

        public TamañoPrecio obtenerTamañoPrecio(int Id)
        {
            return _dbEntities.tamañoPrecios.First(o => o.Id == Id);
        }

        public Productos obtenerProductoId(int IdProducto)
        {
            return _dbEntities.Productos.First(o => o.Id == IdProducto);
        }

        public void editarTamaño(int Id, string TamañoProducto, decimal Precio)
        {
            TamañoPrecio tamañoPrecio1 = _dbEntities.tamañoPrecios.First(o => o.Id == Id);
            tamañoPrecio1.TamañoProducto = TamañoProducto;
            tamañoPrecio1.Precio = Precio;
            _dbEntities.SaveChanges();
        }
        public void eliminarTamañoPrecio(int Id)
        {
            var precioTam = _dbEntities.tamañoPrecios.First(o => o.Id == Id);
            _dbEntities.tamañoPrecios.Remove(precioTam);
            _dbEntities.SaveChanges();
        }

        public Usuario obtenerUsuario(string username)
        {
            return _dbEntities.usuarios.First(o => o.Correo == username);
        }
    }
}
