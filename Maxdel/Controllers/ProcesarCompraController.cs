using Microsoft.AspNetCore.Mvc;
using Maxdel.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Maxdel.Models;
using System.Security.Claims;
using Maxdel.ViewModels;
using Maxdel.Repositorio;

namespace Maxdel.Controllers
{
    public class ProcesarCompraController : Controller
    {
        private readonly DbEntities _dbEntities;
        private readonly IProcesarCompraRepositorio procesarCompraRepositorio;
        public ProcesarCompraController(DbEntities dbEntities, IProcesarCompraRepositorio procesarCompraRepositorio)
        {
            _dbEntities = dbEntities;
            this.procesarCompraRepositorio = procesarCompraRepositorio;
        }
        [HttpGet]
        public IActionResult Detalle(int IdProducto)
        {
            var Producto = procesarCompraRepositorio.obtenerProducto(IdProducto);
            return View(Producto);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Detalle(int IdProducto, ComprarClaseIntermedia clase)
        {
            if (clase.IdProducto == null || clase.Cantidad == null || clase.Cantidad < 1)
            {
                var Producto = procesarCompraRepositorio.obtenerProducto(IdProducto);
                ModelState.AddModelError("Validacion", "Seleccione los campos correctos");
                return View(Producto);
            }
            int Id = GetLoggedUser().Id;
            Pedido pedido = new Pedido();
            DetallePedido detallePedido = new DetallePedido();
            pedido.IdUsuario = Id;
            pedido.Estado = 1;
            pedido.Destino = "Delivery";
            //procesarCompraRepositorio.registrarPedido(Id);
            _dbEntities.pedidos.Add(pedido);
            _dbEntities.SaveChanges();

            int idPedido = procesarCompraRepositorio.obtenerIdPedido();

            detallePedido.IdProducto = IdProducto;
            detallePedido.IdPedido = idPedido;
            detallePedido.IdTamañoPrecio = clase.IdTamañoPrecio;
            detallePedido.Cantidad = clase.Cantidad;
            TamañoPrecio tamañoPrecio = procesarCompraRepositorio.obtenerTamañoPrecio(detallePedido.IdTamañoPrecio);
            detallePedido.TamañoProducto = tamañoPrecio.TamañoProducto;
            detallePedido.precio = tamañoPrecio.Precio;

            //procesarCompraRepositorio.actualizarMonto(idPedido, clase.Cantidad, tamañoPrecio.Precio);
            Pedido pedidoAux2 = new Pedido();
            pedidoAux2 = _dbEntities.pedidos.First(o => o.Id == idPedido);
            pedidoAux2.Monto = clase.Cantidad * tamañoPrecio.Precio;
            _dbEntities.SaveChanges();
            procesarCompraRepositorio.agregarDetalle(detallePedido);

            return RedirectToAction("Cesta", "ProcesarCompra");
        }
        [Authorize]
        public IActionResult Cesta()
        {
            int Id = GetLoggedUser().Id;

            ViewBag.DatosCesta = procesarCompraRepositorio.listarDatosCesta(Id);

            ViewBag.Monto = obtenerMonto(Id);

            ViewBag.Direcciones = procesarCompraRepositorio.obtenerDirecciones(Id);
            return View();
        }

        public decimal obtenerMonto(int Id)
        {
            decimal monto = 0;

            var aux = procesarCompraRepositorio.obtenerMontos(Id);

            foreach (var item in aux)
            {
                monto += item.precio * item.Cantidad;
            }

            return monto;
        }

        [Authorize]
        public IActionResult ActualizarCantidad(int Id, int Cantidad)
        {
            if (Cantidad == null || Cantidad < 0)
            {
                ModelState.AddModelError("cantidad", "Cantidad Invalida");
                return RedirectToAction("Cesta", "ProcesarCompra");
            }
            procesarCompraRepositorio.actualizarCantidad(Id, Cantidad);
            return RedirectToAction("Cesta", "ProcesarCompra");
        }
        private Usuario GetLoggedUser()
        {
            var claim = HttpContext.User.Claims.First();
            string username = claim.Value;
            return procesarCompraRepositorio.obtenerUsuario(username);
        }

        private string generarCodTracking(int Id)
        {
            string codTracking = "MAX";
            Usuario temp = procesarCompraRepositorio.obtenerUsuarioFirst(Id);
            string temp1 = temp.Nombre;
            codTracking += temp1[0];
            temp1 = temp.Apellido;
            codTracking += temp1[0];
            int nro = procesarCompraRepositorio.obtenerSecuenciaTracking();
            temp1 = nro.ToString();
            int cant = temp1.Length;
            int max = 6 - cant;
            for (int i = 0; i < max; i++)
            {
                codTracking += "0";
            }
            codTracking += temp1;
            procesarCompraRepositorio.actualizarCodTracking();
            return codTracking;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Delete(int id)
        {
            procesarCompraRepositorio.eliminarPedido(id);

            return RedirectToAction("Cesta", "ProcesarCompra");
        }
        [Authorize]
        public IActionResult PedidoExito(int IdDireccion)
        {
            if (IdDireccion == null || IdDireccion == 0)
            {
                int Id = GetLoggedUser().Id;

                ViewBag.DatosCesta = procesarCompraRepositorio.listarPedidos(Id);

                ViewBag.Monto = obtenerMonto(Id);

                ViewBag.Direcciones = procesarCompraRepositorio.obtenerDirecciones(Id);
                ModelState.AddModelError("Direccion", "Elija una dirección");
                return View("Cesta", "ProcesarCompra");
            }
            ViewBag.nroTracking = Comprar(IdDireccion);
            return View();
        }

        private string Comprar(int IdDireccion)
        {
            int Id = GetLoggedUser().Id;
            List<Pedido> pedidos = new List<Pedido>();
            pedidos = procesarCompraRepositorio.listarPedidosActualizar(Id);
            int IdBoleta = generarNroBoleta(IdDireccion);
            string codTracking = generarCodTracking(Id);
            for (int i = 0; i < pedidos.Count; i++)
            {
                pedidos[i].Estado = 2;
                pedidos[i].CodTracking = codTracking;
                pedidos[i].IdBoleta = IdBoleta;
            }
            procesarCompraRepositorio.comprar();
            return codTracking;
        }
        private int generarNroBoleta(int IdDireccion)
        {
            string nroBoleta = "001-";
            int aux2;
            int nroTemp = procesarCompraRepositorio.contarBoletas();
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

            int Id = GetLoggedUser().Id;
            
            Boleta boleta = new Boleta();
            boleta.Fecha = fecha;
            boleta.NroBoleta = nroBoleta;
            boleta.MontoTotal = obtenerMonto(Id);
            var direccion = procesarCompraRepositorio.obtenerDireccion(IdDireccion);
            boleta.Direccion = direccion.Direccion;
            boleta.Referencia = direccion.Referencia;
            boleta.IdUsuario = GetLoggedUser().Id;

            _dbEntities.boletas.Add(boleta);
            _dbEntities.SaveChanges();

            aux2 = procesarCompraRepositorio.guardarBoleta(boleta);

            return aux2;
        }
    }
}
