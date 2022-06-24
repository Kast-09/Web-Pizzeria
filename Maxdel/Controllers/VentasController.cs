using Maxdel.DB;
using Maxdel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Maxdel.Repositorio;

namespace Maxdel.Controllers
{
    [Authorize]
    public class VentasController : Controller
    {
        private readonly DbEntities _dbEntities;
        private readonly IVentasRepositorio ventasRepositorio;

        public VentasController(DbEntities dbEntities, IVentasRepositorio ventasRepositorio)
        {
            _dbEntities = dbEntities;
            this.ventasRepositorio = ventasRepositorio;
        }

        public IActionResult Index()
        {
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
            var boletas = ventasRepositorio.listarBoletas();
            
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
            return ventasRepositorio.obtenerUsuario(username);
        }
    }
}
