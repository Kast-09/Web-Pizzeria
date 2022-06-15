using Maxdel.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Maxdel.Models;

namespace Maxdel.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly DbEntities _dbEntities;

        public ProductosController(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }

        public IActionResult Index()
        {
            var ListaProductos = _dbEntities.Productos.ToList();
            ViewBag.Tamaños = _dbEntities.tamañoPrecios.ToList();
            return View(ListaProductos);
        }

        [HttpGet]
        public IActionResult AgregarProducto()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AgregarProducto(Productos productos)
        {
            _dbEntities.Productos.Add(productos);
            _dbEntities.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
