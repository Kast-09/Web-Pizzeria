using Maxdel.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Maxdel.Models;
using Maxdel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Maxdel.Repositorio;

namespace Maxdel.Controllers
{
    [Authorize]
    public class VenderController : Controller
    {

        private readonly DbEntities _dbEntities;
        private readonly IVenderRepositorio venderRepositorio;

        public static List<Pedido>? Pedidos = new List<Pedido>();
        public static List<DetallePedido>? DetallePedidos = new List<DetallePedido>();
        public static int cantPedidos = 0;
        public static int cont = 0;

        public VenderController(DbEntities dbEntities, IVenderRepositorio venderRepositorio)
        {
            _dbEntities = dbEntities;
            this.venderRepositorio = venderRepositorio;
        }

        public IActionResult Index(string buscar)
        {
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
            var ListaProductos = venderRepositorio.obtenerProductos();

            if (buscar != null && buscar != "")
            {
                ListaProductos = ListaProductos.Where(o => o.Nombre.Contains(buscar, StringComparison.OrdinalIgnoreCase) || o.Descripcion.Contains(buscar, StringComparison.OrdinalIgnoreCase)).OrderBy(o => o.Nombre).ToList();
            }
            ViewBag.Buscar = buscar;

            var productos = new List<Productos>();

            var ids = new List<int>();

            foreach(var item in DetallePedidos)
            {
                ids.Add(item.Id);
                var detalle = venderRepositorio.obtenerProducto(item.IdProducto);
                productos.Add(detalle);
            }

            ViewBag.productos = productos;

            ViewBag.Cesta = DetallePedidos.ToList();

            ViewBag.ids = ids;

            ViewBag.Monto = obtenerMonto();

            return View(ListaProductos);
        }

        public decimal obtenerMonto()
        {
            decimal monto = 0;

            foreach (var item in DetallePedidos)
            {
                monto += item.precio * item.Cantidad;
            }

            return monto;
        }

        public IActionResult AgregarPedido(int IdProducto, int IdTamañoPrecio, int Cantidad)
        {
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
            if (IdTamañoPrecio == null || Cantidad == null || IdTamañoPrecio == 0 || Cantidad == 0)
            {
                return RedirectToAction("Index");
            }
            Pedido pedido = new Pedido();
            DetallePedido detallePedido = new DetallePedido();
            cantPedidos = venderRepositorio.obtenerUltimoId();
            int aux = 0;
            if(Pedidos.Count == 0)
            {
                aux = 1;
            }
            else aux = Pedidos.Count() + 1;
            cantPedidos += aux;
            pedido.Id = cont;
            pedido.IdUsuario = 0;
            pedido.Estado = 1;
            pedido.Destino = "Tienda";

            detallePedido.Id = cont;
            cont++;
            detallePedido.IdProducto = IdProducto;
            detallePedido.IdPedido = cantPedidos;
            detallePedido.IdTamañoPrecio = IdTamañoPrecio;
            detallePedido.Cantidad = Cantidad;
            TamañoPrecio tamañoPrecio = venderRepositorio.obtenerDetalle(detallePedido.IdTamañoPrecio);
            detallePedido.TamañoProducto = tamañoPrecio.TamañoProducto;
            detallePedido.precio = tamañoPrecio.Precio;
            DetallePedidos.Add(detallePedido);

            pedido.Monto = Cantidad * tamañoPrecio.Precio;
            Pedidos.Add(pedido);
            return RedirectToAction("Index");
        }

        public IActionResult EliminarPedido(int Id)
        {
            DetallePedidos.Remove(DetallePedidos[Id]);
            Pedidos.Remove(Pedidos[Id]);
            return RedirectToAction("Index");
        }

        public IActionResult Limpiar()
        {
            DetallePedidos.Clear();
            Pedidos.Clear();
            cont = 0;
            return RedirectToAction("Index");
        }

        public IActionResult Comprar(ClienteClaseIntermedia cliente)
        {
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Comprar", "Los Datos del cliente no pueden ser vacíos");
                ModelState.AddModelError("Cesta", "Los Datos del cliente no pueden ser vacíos");
                return RedirectToAction("Index");
            }
            if (DetallePedidos.Count == 0 || Pedidos.Count == 0)
            {
                ModelState.AddModelError("Cesta", "Los Datos del cliente no pueden ser vacíos");
                return RedirectToAction("Index");
            }

            venderRepositorio.agregarUsuario(cliente);

            int idUser = venderRepositorio.obtenerUltimoIdUser();

            int IdBoleta = generarNroBoleta(cliente.Direccion);
            for (int i = 0; i < Pedidos.Count; i++)
            {
                Pedidos[i].Id = 0;
                DetallePedidos[i].Id = 0;
                Pedidos[i].IdUsuario = idUser;
                Pedidos[i].Estado = 2;
                Pedidos[i].IdBoleta = IdBoleta;
            }

            venderRepositorio.agregarDatosBd(Pedidos, DetallePedidos);

            DetallePedidos.Clear();
            Pedidos.Clear();

            return RedirectToAction("Index");
        }

        private int generarNroBoleta(string destino)
        {
            string nroBoleta = "001-";
            int aux2;
            int nroTemp = venderRepositorio.contarBoletas();
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

            int Id = venderRepositorio.obtenerUltimoIdUser();
            
            Boleta boleta = new Boleta();
            boleta.Fecha = fecha;
            boleta.NroBoleta = nroBoleta;
            boleta.MontoTotal = obtenerMonto();
            boleta.Direccion = destino;
            boleta.IdUsuario = Id;

            venderRepositorio.agregarBoleta(boleta);

            aux2 = venderRepositorio.obtenerUltimoIdBoleta();

            return aux2;
        }
        private Usuario GetLoggedUser()
        {
            var claim = HttpContext.User.Claims.First();
            string username = claim.Value;
            return venderRepositorio.obtenerUsuario(username);
        }
    }
}
