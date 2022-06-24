using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Maxdel.Models;
using Maxdel.DB;

namespace Maxdel.Controllers
{
    [Authorize]
    public class HomeAdminController : Controller
    {
        private readonly DbEntities _dbEntities;

        public HomeAdminController (DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
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
            return _dbEntities.usuarios.First(o => o.Correo == username);
        }
    }
}
