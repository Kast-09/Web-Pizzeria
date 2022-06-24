using Maxdel.DB;
using Maxdel.Models;
using Maxdel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Maxdel.Controllers
{
    [Authorize]
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
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
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
                user.Contraseña = Convertirsha256(account.Contraseña);
                user.DNI = account.DNI;
                user.IdPreguntaSeguridad = account.IdPreguntaSeguridad;
                user.RespuestaPS = account.RespuestaPS;
                _dbEntities.usuarios.Add(user);
                _dbEntities.SaveChanges();
                return RedirectToAction("Index","HomeAdmin");
            }
            ViewBag.PreguntasSeguridad = _dbEntities.preguntaSeguridads.ToList();
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
            return _dbEntities.usuarios.First(o => o.Correo == username);
        }
    }
}
