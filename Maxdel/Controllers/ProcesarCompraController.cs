using Microsoft.AspNetCore.Mvc;
using Maxdel.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Maxdel.Models;
using System.Security.Claims;
using Maxdel.ViewModels;

namespace Maxdel.Controllers
{
    public class ProcesarCompraController : Controller
    {
        DbEntities _dbEntities;
        public ProcesarCompraController(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }
        [HttpGet]
        public IActionResult Detalle(int IdProducto)
        {
            var Producto = _dbEntities.Productos
                                .Include("TamañoPrecios")
                                .First(o => o.Id == IdProducto);
            return View(Producto);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Detalle(int IdProducto, ComprarClaseIntermedia clase)
        {
            if (clase.IdProducto == null || clase.Cantidad == null || clase.Cantidad < 1)
            {
                var Producto = _dbEntities.Productos
                                .Include("TamañoPrecios")
                                .First(o => o.Id == IdProducto);
                ModelState.AddModelError("Validacion", "Seleccione los campos correctos");
                return View(Producto);
            }
            int Id = GetLoggedUser().Id;
            Pedido pedido = new Pedido();
            DetallePedido detallePedido = new DetallePedido();
            pedido.IdUsuario = Id;
            pedido.Estado = 1;
            pedido.Destino = "Delivery";
            _dbEntities.pedidos.Add(pedido);
            _dbEntities.SaveChanges();

            int idPedido = _dbEntities.pedidos.OrderBy(o => o.Id).Last().Id;

            detallePedido.IdProducto = IdProducto;
            detallePedido.IdPedido = idPedido;
            detallePedido.IdTamañoPrecio = clase.IdTamañoPrecio;
            detallePedido.Cantidad = clase.Cantidad;
            TamañoPrecio tamañoPrecio = _dbEntities.tamañoPrecios.First(o => o.Id == detallePedido.IdTamañoPrecio);
            detallePedido.TamañoProducto = tamañoPrecio.TamañoProducto;
            detallePedido.precio = tamañoPrecio.Precio;
            Pedido pedidoAux2 = new Pedido();
            pedidoAux2 = _dbEntities.pedidos.First(o => o.Id == idPedido);
            pedidoAux2.Monto = clase.Cantidad * tamañoPrecio.Precio;
            _dbEntities.SaveChanges();
            _dbEntities.detallePedidos.Add(detallePedido);
            _dbEntities.SaveChanges();

            return RedirectToAction("Cesta", "ProcesarCompra");
        }
        [Authorize]
        public IActionResult Cesta()
        {
            int Id = GetLoggedUser().Id;

            ViewBag.DatosCesta = _dbEntities.detallePedidos
                    .Include(o => o.Producto)
                    .Include(o => o.Pedido)
                    .Include(o => o.Pedido.EstadoFK)
                    .Where(o => o.Pedido.IdUsuario == Id && o.Pedido.Estado == 1).ToList();

            ViewBag.Monto = obtenerMonto(Id);

            ViewBag.Direcciones = _dbEntities.direcciones
                    .Where(o => o.IdUsuario == Id);
            return View();
        }

        public decimal obtenerMonto(int Id)
        {
            decimal monto = 0;

            var aux = _dbEntities.detallePedidos
                    .Include(o => o.Pedido)
                    .Where(o => o.Pedido.IdUsuario == Id && o.Pedido.Estado == 1).ToList();

            foreach (var item in aux)
            {
                monto += item.precio * item.Cantidad;
            }

            return monto;
        }

        [Authorize]
        public IActionResult ActualizarCantidad(int Id, int Cantidad)
        {
            if(Cantidad == null || Cantidad < 0)
            {
                ModelState.AddModelError("cantidad", "Cantidad Invalida");
                return RedirectToAction("Cesta", "ProcesarCompra");
            }
            DetallePedido pedido = _dbEntities.detallePedidos.First(o => o.Id == Id);
            pedido.Cantidad = Cantidad;
            _dbEntities.SaveChanges();
            return RedirectToAction("Cesta", "ProcesarCompra");
        }
        private Usuario GetLoggedUser()
        {
            var claim = HttpContext.User.Claims.First();
            string username = claim.Value;
            return _dbEntities.usuarios.First(o => o.Correo == username);
        }

        private string generarCodTracking(int Id)
        {
            string codTracking = "MAX";
            Usuario temp = _dbEntities.usuarios.First(o => o.Id == Id);
            string temp1 = temp.Nombre;
            codTracking += temp1[0];
            temp1 = temp.Apellido;
            codTracking += temp1[0];
            int nro = _dbEntities.nroTrackings.First(o => o.Id == 1).Numero;
            temp1 = nro.ToString();
            int cant = temp1.Length;
            int max = 6 - cant;
            for(int i = 0; i < max; i++)
            {
                codTracking += "0";
            }
            codTracking += temp1;
            NroTracking nroTracking = new NroTracking();
            nroTracking = _dbEntities.nroTrackings.First(o => o.Id == 1);
            nroTracking.Numero++;
            _dbEntities.SaveChanges();
            return codTracking;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var pedidoDb = _dbEntities.pedidos.First(o => o.Id == id);
            _dbEntities.pedidos.Remove(pedidoDb);
            _dbEntities.SaveChanges();
            var detalleDb = _dbEntities.detallePedidos.First(o => o.IdPedido == id);
            _dbEntities.detallePedidos.Remove(detalleDb);
            _dbEntities.SaveChanges();

            return RedirectToAction("Cesta", "ProcesarCompra");
        }
        [Authorize]
        public IActionResult PedidoExito(int IdDireccion)
        {
            if(IdDireccion == null || IdDireccion == 0)
            {
                int Id = GetLoggedUser().Id;

                ViewBag.DatosCesta = _dbEntities.detallePedidos
                        .Include(o => o.Producto)
                        .Include(o => o.Pedido)
                        .Include(o => o.Pedido.EstadoFK)
                        .Where(o => o.Pedido.IdUsuario == Id && o.Pedido.Estado == 1).ToList();

                ViewBag.Monto = obtenerMonto(Id);

                ViewBag.Direcciones = _dbEntities.direcciones
                        .Where(o => o.IdUsuario == Id);
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
            pedidos = _dbEntities.pedidos.Where(o => o.IdUsuario == Id && o.Estado == 1).ToList();
            int IdBoleta = generarNroBoleta(IdDireccion);
            string codTracking = generarCodTracking(Id);
            for (int i = 0; i < pedidos.Count; i++)
            {
                pedidos[i].Estado = 2;
                pedidos[i].CodTracking = codTracking;
                pedidos[i].IdBoleta = IdBoleta;
            }
            _dbEntities.SaveChanges();
            return codTracking;
        }
        private int generarNroBoleta(int IdDireccion)
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

            int Id = GetLoggedUser().Id;
            var montos = _dbEntities.pedidos.Where(o => o.IdUsuario == Id && o.Estado == 1).ToList();
            decimal monto = 0;

            foreach (var item in montos)
            {
                monto += (decimal)item.Monto;
            }

            Boleta boleta = new Boleta();
            boleta.Fecha = fecha;
            boleta.NroBoleta = nroBoleta;
            boleta.MontoTotal = monto;
            var direccion = _dbEntities.direcciones.First(o => o.Id == IdDireccion);
            boleta.Direccion = direccion.Direccion;
            boleta.Referencia = direccion.Referencia;
            boleta.IdUsuario = GetLoggedUser().Id;

            _dbEntities.boletas.Add(boleta);
            _dbEntities.SaveChanges();

            aux2 = _dbEntities.boletas.OrderBy(o => o.Id).Last().Id;

            return aux2;
        }
    }
}
