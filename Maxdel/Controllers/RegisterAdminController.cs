using Maxdel.DB;
using Maxdel.Models;
using Maxdel.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Maxdel.Controllers
{
    public class RegisterAdminController : Controller
    {
        private readonly DbEntities _dbEntities;

        public RegisterAdminController(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.PreguntasSeguridad = _dbEntities.preguntaSeguridads.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Register(AgregarAdminClaseIntermedia account) // POST
        {
            Usuario user = new Usuario();

            if (ModelState.IsValid)
            {
                user.IdRol = 1;
                user.Nombre = account.Nombre;
                user.Apellido = account.Apellido;
                user.NroCelular = account.NroCelular;
                user.Correo = account.Correo;
                user.Contraseña = account.Contraseña;
                user.DNI = account.DNI;
                _dbEntities.usuarios.Add(user);
                _dbEntities.SaveChanges();
                return RedirectToAction("Index","HomeAdmin");
            }
            return View("Register", "RegisterAdmin");
        }
    }
}
