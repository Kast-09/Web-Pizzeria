using Maxdel.DB;
using Maxdel.Models;
using Maxdel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Maxdel.Repositorio;

namespace Maxdel.Controllers
{
    [Authorize]
    public class RegisterAdminController : Controller
    {
        private readonly DbEntities _dbEntities;
        private readonly IRegisterAdminRepositorio registerAdminRepositorio;

        public RegisterAdminController(DbEntities dbEntities, IRegisterAdminRepositorio registerAdminRepositorio)
        {
            _dbEntities = dbEntities;
            this.registerAdminRepositorio = registerAdminRepositorio;
        }

        [HttpGet]
        public IActionResult Register()
        {
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
            ViewBag.PreguntasSeguridad = registerAdminRepositorio.listarPreguntas();
            return View();
        }

        [HttpPost]
        public IActionResult Register(AgregarAdminClaseIntermedia account) // POST
        {
            if (ModelState.IsValid)
            {
                account.Contraseña = Convertirsha256(account.Contraseña);
                registerAdminRepositorio.guardarUser(account);
                return RedirectToAction("Index","HomeAdmin");
            }
            ViewBag.PreguntasSeguridad = registerAdminRepositorio.listarPreguntas();
            return View("Register");
        }

        public static string Convertirsha256(string texto)
        {
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));
                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));

            }

            return Sb.ToString();
        }

        private Usuario GetLoggedUser()
        {
            var claim = HttpContext.User.Claims.First();
            string username = claim.Value;
            return registerAdminRepositorio.obtenerUsuario(username);
        }
    }
}
