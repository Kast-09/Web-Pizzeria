using Maxdel.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maxdel.Controllers
{
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
    }
}
