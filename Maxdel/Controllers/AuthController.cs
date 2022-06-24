using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Maxdel.DB;
using Maxdel.DB.Mapping;
using Maxdel.Models;
using Maxdel.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;
using System.Security.Cryptography;
using Maxdel.Repositorio;

namespace Maxdel.Controllers
{
    public class AuthController : Controller
    {
        private readonly DbEntities _dbEntities;
        private readonly IAuthRepositorio authRepositorio;

        public AuthController(DbEntities dbEntities, IAuthRepositorio authRepositorio)
        {
            _dbEntities = dbEntities;
            this.authRepositorio = authRepositorio;
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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string correo, string contraseña)
        {
            if(correo == null || correo == "" || contraseña == null || contraseña == "")
            {
                return View("Login");
            }
            contraseña = Convertirsha256(contraseña);
            if (authRepositorio.verificar(correo, contraseña))
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, correo),
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                HttpContext.SignInAsync(claimsPrincipal);

                var usuario = authRepositorio.obtenerUsuario(correo, contraseña);

                if (usuario.IdRol == 1)
                {
                    return RedirectToAction("Index", "HomeAdmin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }                
            }            
            else ModelState.AddModelError("AuthError", "Usuario y/o contraseña erronea");
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.PreguntasSeguridad = authRepositorio.obtenerPreguntas();
            return View(new RegistroClaseIntermedia());
        }

        [HttpPost]
        public IActionResult Register(RegistroClaseIntermedia account) // POST
        {
            int aux = authRepositorio.contarUsuarios();
            Usuario user = new Usuario();
            Direcciones direcciones = new Direcciones();

            if (ModelState.IsValid)
            {
                user.IdRol = 2;
                user.Nombre = account.Nombre;
                user.Apellido = account.Apellidos;
                user.NroCelular = account.NroCelular;
                user.Correo = account.correo;
                user.Contraseña = Convertirsha256(account.contraseña);
                user.IdPreguntaSeguridad = account.IdPreguntaSeguridad;
                user.RespuestaPS = account.RespuestaPS;
                direcciones.IdUsuario = authRepositorio.obtenerUltimoUsuaro();
                direcciones.Direccion = account.Direccion;
                direcciones.Referencia = account.Referencia;
                authRepositorio.registrar(user, direcciones);
                return RedirectToAction("Login");
            }
            ViewBag.PreguntasSeguridad = _dbEntities.preguntaSeguridads.ToList();
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
            if (!ModelState.IsValid)
            {                
                return View("Correo");
            }
            Usuario? user = authRepositorio.obtenerUsuario(correo);
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
            if (Id == null || IdPreguntaSeguridad == null || RespuestaPS == null || RespuestaPS == "")
            {
                ViewBag.PreguntasSeguridad = _dbEntities.preguntaSeguridads.ToList();
                ViewBag.Id = Id;
                ModelState.AddModelError("Vacio", "Las campos no pueden ser vacíos");
                return View("PreguntaSeguridad", Id);
            }
            if (authRepositorio.verificarPreguntaSeguridad(Id, IdPreguntaSeguridad, RespuestaPS))
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
            if(Id == null || contraseña == null || contraseña == "" || contraseñaV == null || contraseñaV == "")
            {
                ModelState.AddModelError("Vacio", "Las campos no pueden ser vacíos");
                ViewBag.Id = Id;
                return View("ActualizarContraseña", Id);
            }
            if(contraseña == contraseñaV)
            {
                contraseña = Convertirsha256(contraseña);
                authRepositorio.actualizarUsuario(Id, contraseña);
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
