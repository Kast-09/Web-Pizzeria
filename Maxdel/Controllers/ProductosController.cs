using Maxdel.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Maxdel.Models;
using Maxdel.Repositorio;

namespace Maxdel.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly DbEntities _dbEntities;
        private readonly IProductosRepositorios productosRepositorios;

        public ProductosController(DbEntities dbEntities, IProductosRepositorios productosRepositorios)
        {
            _dbEntities = dbEntities;
            this.productosRepositorios = productosRepositorios;
        }

        public IActionResult Index()
        {
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
            var ListaProductos = productosRepositorios.listarProductos();
            return View(ListaProductos);
        }

        [HttpGet]
        public IActionResult AgregarProducto()
        {
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
            return View();
        }
        [HttpPost]
        public IActionResult AgregarProducto(Productos productos)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("AgregarProducto", "Rellene los datos");
                return View("AgregarProducto");
            }
            productosRepositorios.agregarProducto(productos);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditarProducto(int Id)
        {
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
            var Producto = productosRepositorios.obtenerProducto(Id);
            return View(Producto);
        }
        [HttpPost]
        public IActionResult EditarProducto(int Id, Productos productos)
        {
            if (!ModelState.IsValid)
            {
                var Producto = productosRepositorios.obtenerProducto(Id);
                ModelState.AddModelError("EditarProducto", "Rellene los datos");
                return View("EditarProducto", Producto);
            }

            productosRepositorios.editarProducto(Id, productos);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AgregarTamañoPrecio(int Id)
        {
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
            var Producto = productosRepositorios.obtenerProducto(Id);
            return View(Producto);
        }
        [HttpPost]
        public IActionResult AgregarTamañoPrecio(int Id, TamañoPrecio tamañoPrecio)
        {
            if (tamañoPrecio.TamañoProducto == null || tamañoPrecio.TamañoProducto == "")
            {
                var Producto = productosRepositorios.obtenerProducto(Id);
                ModelState.AddModelError("Agregar", "Rellene los datos");
                return View("AgregarTamañoPrecio", Producto);
            }
            if (tamañoPrecio.Precio <= 0)
            {
                var Producto = productosRepositorios.obtenerProducto(Id);
                ModelState.AddModelError("Agregar", "Rellene los datos");
                return View("AgregarTamañoPrecio", Producto);
            }
            productosRepositorios.AgregarTamañoPrecio(Id, tamañoPrecio);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditarTamañoPrecio(int Id)
        {
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
            TamañoPrecio tamañoPrecio = productosRepositorios.obtenerTamañoPrecio(Id);
            ViewBag.Producto = productosRepositorios.obtenerProductoId(tamañoPrecio.IdProducto);
            return View(tamañoPrecio);
        }
        [HttpPost]
        public IActionResult EditarTamañoPrecio(int Id, TamañoPrecio tamañoPrecio)
        {
            if (!ModelState.IsValid)
            {
                TamañoPrecio tamañoPrecioo = productosRepositorios.obtenerTamañoPrecio(Id);
                ViewBag.Producto = productosRepositorios.obtenerProductoId(tamañoPrecioo.IdProducto);
                return View("EditarTamañoPrecio", tamañoPrecio);
            }
            if (tamañoPrecio.Precio < 0)
            {
                TamañoPrecio tamañoPrecioo = productosRepositorios.obtenerTamañoPrecio(Id);
                ViewBag.Producto = productosRepositorios.obtenerProductoId(tamañoPrecioo.IdProducto);
                return View("EditarTamañoPrecio", tamañoPrecio);
            }
            productosRepositorios.editarTamaño(Id, tamañoPrecio.TamañoProducto, tamañoPrecio.Precio);
            return RedirectToAction("Index");
        }
        public IActionResult EliminarTamañoPrecio(int Id)
        {
            productosRepositorios.eliminarTamañoPrecio(Id);
            return RedirectToAction("Index");
        }

        private Usuario GetLoggedUser()
        {
            var claim = HttpContext.User.Claims.First();
            string username = claim.Value;
            return productosRepositorios.obtenerUsuario(username);
        }
    }
}
