using Maxdel.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maxdel.Controllers
{
    public class VentasController : Controller
    {
        private readonly DbEntities _dbEntities;

        public VentasController(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }

        public IActionResult Index()
        {
            var boletas = _dbEntities.boletas
                                .Include("Pedidos")
                                .Where(o => o.Pedidos.Any(x => x.Estado == 5)).ToList();
            
            decimal subtotal = 0;

            foreach (var item in boletas)
            {
                subtotal += item.MontoTotal;
            }

            ViewBag.Subtotal = subtotal;

            return View(boletas);
        }
    }
}
