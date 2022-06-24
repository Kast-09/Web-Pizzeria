using Maxdel.DB;
using Maxdel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maxdel.Controllers
{
    [Authorize]
    public class VentasController : Controller
    {
        private readonly DbEntities _dbEntities;

        public VentasController(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }

        public IActionResult Index()
        {
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
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

        private Usuario GetLoggedUser()
        {
            var claim = HttpContext.User.Claims.First();
            string username = claim.Value;
            return _dbEntities.usuarios.First(o => o.Correo == username);
        }
    }
}
