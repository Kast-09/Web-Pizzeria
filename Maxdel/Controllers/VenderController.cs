using Maxdel.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Maxdel.Models;
using Maxdel.ViewModels;

namespace Maxdel.Controllers
{
    public class VenderController : Controller
    {

        private readonly DbEntities _dbEntities;

        public static List<Pedido>? Pedidos = new List<Pedido>();
        public static List<DetallePedido>? DetallePedidos = new List<DetallePedido>();
        public static int cantPedidos = 0;

        public VenderController(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }

        public IActionResult Index(string buscar)
        {
            var ListaProductos = _dbEntities.Productos
                                        .Include("TamañoPrecios")
                                        .ToList();

            if (buscar != null && buscar != "")
            {
                ListaProductos = ListaProductos.Where(o => o.Nombre.Contains(buscar, StringComparison.OrdinalIgnoreCase) || o.Descripcion.Contains(buscar, StringComparison.OrdinalIgnoreCase)).OrderBy(o => o.Nombre).ToList();
            }
            ViewBag.Buscar = buscar;

            var productos = new List<Productos>();

            foreach(var item in DetallePedidos)
            {
                var detalle = _dbEntities.Productos.FirstOrDefault(o => o.Id == item.IdProducto);
                productos.Add(detalle);
            }

            ViewBag.productos = productos;

            ViewBag.Cesta = DetallePedidos.ToList();

            return View(ListaProductos);
        }

        public IActionResult AgregarPedido(int IdProducto, int IdTamañoPrecio, int Cantidad)
        {
            Pedido pedido = new Pedido();
            DetallePedido detallePedido = new DetallePedido();
            cantPedidos = _dbEntities.pedidos.OrderBy(o => o.Id).Last().Id;
            int aux = 0;
            if(Pedidos.Count == 0)
            {
                aux = 1;
            }
            else aux = Pedidos.Count() + 1;
            cantPedidos += aux;
            pedido.IdUsuario = 0;
            pedido.Estado = 1;
            pedido.Destino = "Tienda";

            detallePedido.IdProducto = IdProducto;
            detallePedido.IdPedido = cantPedidos;
            detallePedido.IdTamañoPrecio = IdTamañoPrecio;
            detallePedido.Cantidad = Cantidad;
            TamañoPrecio tamañoPrecio = _dbEntities.tamañoPrecios.First(o => o.Id == detallePedido.IdTamañoPrecio);
            detallePedido.TamañoProducto = tamañoPrecio.TamañoProducto;
            detallePedido.precio = tamañoPrecio.Precio;
            DetallePedidos.Add(detallePedido);

            pedido.Monto = Cantidad * tamañoPrecio.Precio;
            Pedidos.Add(pedido);
            return RedirectToAction("Index");
        }

        public IActionResult Limpiar()
        {
            DetallePedidos.Clear();
            Pedidos.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Comprar(ClienteClaseIntermedia cliente)
        {
            Usuario usuario = new Usuario();
            usuario.IdRol = 3;
            usuario.Nombre = cliente.Nombre;
            usuario.Apellido = cliente.Apellido;
            usuario.DNI = cliente.DNI;

            _dbEntities.usuarios.Add(usuario);
            _dbEntities.SaveChanges();

            int idUser = _dbEntities.usuarios.OrderBy(o => o.Id).Last().Id;

            int IdBoleta = generarNroBoleta(cliente.Direccion);
            for (int i = 0; i < Pedidos.Count; i++)
            {
                Pedidos[i].IdUsuario = idUser;
                Pedidos[i].Estado = 2;
                Pedidos[i].IdBoleta = IdBoleta;
            }
            _dbEntities.pedidos.AddRange(Pedidos);
            _dbEntities.detallePedidos.AddRange(DetallePedidos);
            _dbEntities.SaveChanges();

            DetallePedidos.Clear();
            Pedidos.Clear();

            return RedirectToAction("Index");
        }

        private int generarNroBoleta(string destino)
        {
            string nroBoleta = "001-";
            int aux2;
            int nroTemp = _dbEntities.boletas.Count();
            nroTemp++;
            string aux = nroTemp.ToString();
            nroTemp = aux.Length;
            nroTemp = 7 - nroTemp;
            for (int i = 0; i < nroTemp; i++)
            {
                nroBoleta += "0";
            }
            nroBoleta += aux;

            DateTime fecha = DateTime.Now;

            int Id = _dbEntities.usuarios.OrderBy(o => o.Id).Last().Id;
            decimal monto = 0;

            for (int i = 0; i < DetallePedidos.Count; i++)
            {
                monto += DetallePedidos[i].precio;
            }

            Boleta boleta = new Boleta();
            boleta.Fecha = fecha;
            boleta.NroBoleta = nroBoleta;
            boleta.MontoTotal = monto;
            boleta.Direccion = destino;
            boleta.IdUsuario = Id;

            _dbEntities.boletas.Add(boleta);
            _dbEntities.SaveChanges();

            aux2 = _dbEntities.boletas.OrderBy(o => o.Id).Last().Id;

            return aux2;
        }
    }
}
