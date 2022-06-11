using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Maxdel.DB;
using Maxdel.DB.Mapping;
using Maxdel.Models;
using Maxdel.ViewModels;

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
            return View();
        }
        public IActionResult Prueba()
        {
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
                direcciones.IdUsuario = aux + 2;
                direcciones.Direccion = account.Direccion;
                direcciones.Referencia = account.Referencia;
                _dbEntities.usuarios.Add(user);
                _dbEntities.direcciones.Add(direcciones);
                _dbEntities.SaveChanges();
                TempData["SuccessMessage"] = "Se creo el usuario de manera exitosa";
                return RedirectToAction("Login");
            }
            TempData["SuccessMessage"] = "No se creo el usuario";
            return View("Register", account);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
