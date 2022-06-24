using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Maxdel.Models;
using Maxdel.DB;
using Maxdel.Repositorio;

namespace Maxdel.Controllers
{
    [Authorize]
    public class HomeAdminController : Controller
    {
        private readonly DbEntities _dbEntities;
        private readonly IHomeAdminRepositorio homeAdminRepositorio;

        public HomeAdminController (DbEntities dbEntities, IHomeAdminRepositorio homeAdminRepositorio)
        {
            _dbEntities = dbEntities;
            this.homeAdminRepositorio = homeAdminRepositorio;
        }

        public IActionResult Index()
        {
            int IdRol = GetLoggedUser().IdRol;
            if(IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
            return View();
        }

        private Usuario GetLoggedUser()
        {
            var claim = HttpContext.User.Claims.First();
            string username = claim.Value;
            return homeAdminRepositorio.obtenerUsuario(username);
        }
    }
}
