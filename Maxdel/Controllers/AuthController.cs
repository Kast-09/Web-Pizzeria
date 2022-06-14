using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Maxdel.DB;
using Maxdel.DB.Mapping;
using Maxdel.Models;
using Maxdel.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Maxdel.Controllers
{
    public class AuthController : Controller
    {
        private DbEntities _dbEntities;

        public AuthController(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string correo, string contraseña)
        {
            if (_dbEntities.usuarios.Any(x => x.Correo == correo && x.Contraseña == contraseña))
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, correo),
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                HttpContext.SignInAsync(claimsPrincipal);

                var usuario = _dbEntities.usuarios.First(x => x.Correo == correo && x.Contraseña == contraseña);

                if(usuario.IdTipo == 1)
                {
                    return RedirectToAction("Index", "HomeAdmin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }                
            }
            if (correo == null) ModelState.AddModelError("Correo", "El campo correo no puede ser vacío.");
            else if (contraseña == null) ModelState.AddModelError("Contraseña", "El campo contraseña no puede ser vacío.");
            else ModelState.AddModelError("AuthError", "Usuario y/o contraseña erronea");
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.PreguntasSeguridad = _dbEntities.preguntaSeguridads.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegistroClaseIntermedia account) // POST
        {
            int aux = _dbEntities.usuarios.Count();
            Usuario user = new Usuario();
            Direcciones direcciones = new Direcciones();

            if (ModelState.IsValid)
            {
                user.IdTipo = 2;
                user.Nombre = account.Nombre;
                user.Apellido = account.Apellidos;
                user.NroCelular = account.NroCelular;
                user.Correo = account.correo;
                user.Contraseña = account.contraseña;
                user.IdPreguntaSeguridad = account.IdPreguntaSeguridad;
                user.RespuestaPS = account.RespuestaPS;
                direcciones.IdUsuario = aux + 2;
                direcciones.Direccion = account.Direccion;
                direcciones.Referencia = account.Referencia;
                _dbEntities.usuarios.Add(user);
                _dbEntities.direcciones.Add(direcciones);
                _dbEntities.SaveChanges();
                return RedirectToAction("Login");
            }
            return View("Register", account);
        }
        [HttpGet]
        public IActionResult Correo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Correo(string correo)
        {
            Usuario? user = _dbEntities.usuarios.FirstOrDefault(o => o.Correo == correo);
            if (user != null)
            {
                return RedirectToAction("PreguntaSeguridad", new { Id = user.Id });
            }
            else ModelState.AddModelError("Correo", "Correo no encontrado");
            return View();
        }
        [HttpGet]
        public IActionResult PreguntaSeguridad(int Id)
        {
            ViewBag.PreguntasSeguridad = _dbEntities.preguntaSeguridads.ToList();
            ViewBag.Id = Id;
            return View();
        }
        [HttpPost]
        public IActionResult PreguntaSeguridad(int Id, int IdPreguntaSeguridad, string RespuestaPS)
        {
            if (_dbEntities.usuarios.Any(o => o.Id == Id
                       && o.IdPreguntaSeguridad == IdPreguntaSeguridad
                       && o.RespuestaPS == RespuestaPS))
            {
                return RedirectToAction("ActualizarContraseña", new { Id = Id });
            }
            ModelState.AddModelError("ErrorPregunta", "Datos Incorrectos");
            return RedirectToAction("PreguntaSeguridad", new { Id = Id });            
        }
        [HttpGet]
        public IActionResult ActualizarContraseña(int Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        [HttpPost]
        public IActionResult ActualizarContraseña(int Id, string contraseña, string contraseñaV)
        {
            if(contraseña == contraseñaV)
            {
                Usuario user = _dbEntities.usuarios.First(o => o.Id == Id);
                user.Contraseña = contraseña;
                _dbEntities.SaveChanges(); 
                ModelState.AddModelError("Actualizar", "Contraseña actualizada");
                return RedirectToAction("Login", "Auth");
            }
            else ModelState.AddModelError("contraseña", "Las contraseñas no coinciden");
            return RedirectToAction("ActualizarContraseña", new { Id = Id });
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
