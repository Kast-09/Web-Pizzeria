using Microsoft.AspNetCore.Mvc;
using Maxdel.Models;
using System.Diagnostics;
using Maxdel.DB;
using Microsoft.AspNetCore.Authorization;
using Maxdel.Repositorio;
using System.Text;
using System.Security.Cryptography;

namespace Maxdel.Controllers
{
    [Authorize]
    public class PersonaController : Controller
    {
        private readonly DbEntities _dbEntities;
        private readonly IPersonaRepositorio personaRepositorio;

        public PersonaController(DbEntities dbEntities, IPersonaRepositorio personaRepositorio)
        {
            _dbEntities = dbEntities;
            this.personaRepositorio = personaRepositorio;
        }

        public IActionResult Index()
        {
            int Id = GetLoggedUser().Id;
            var persona = personaRepositorio.obtenerUsuarioId(Id);
            ViewBag.Direcciones = personaRepositorio.obtenerDirecciones(Id);
            return View(persona);
        }
        [HttpGet]
        public IActionResult EditarPersona()
        {
            int id = GetLoggedUser().Id;
            var persona = personaRepositorio.obtenerUsuarioId(id);
            return View(persona);
        }
        [HttpPost]
        public IActionResult EditarPersona(int id, Usuario user)
        {
            if (user.Nombre == null || user.Nombre == "" 
                || user.Apellido == null || user.Apellido == "" 
                || user.NroCelular == null || user.NroCelular == "" 
                || user.Correo == null || user.Correo == "")
            {
                var persona = _dbEntities.usuarios.First(o => o.Id == id);
                ModelState.AddModelError("Editar", "Datos erroneos");
                return View("EditarPersona", persona);
            }
            var Persona = personaRepositorio.obtenerUsuarioId(id);
            Persona.Nombre = user.Nombre;
            Persona.Apellido = user.Apellido;
            Persona.NroCelular = user.NroCelular;
            Persona.Correo = user.Correo;
            personaRepositorio.actualizarBD();
            return RedirectToAction("Index", "Persona");
        }
        public IActionResult AgregaDireccion()
        {
            return View();
        }
        public IActionResult NuevaDireccion(int retorno, string Direccion, string Referencia)
        {
            if (Direccion == null || Direccion == "" || Referencia == null || Referencia == "")
            {
                ModelState.AddModelError("Excepcion", "Los campos no pueden ser vacíos");
                return View("AgregaDireccion");
            }
            int Id = GetLoggedUser().Id;
            Direcciones direccion = new Direcciones();
            direccion.IdUsuario = Id;
            direccion.Direccion = Direccion;
            direccion.Referencia = Referencia;
            personaRepositorio.agregarDireccion(direccion);
            if(retorno == 1) return RedirectToAction("Index", "Persona");
            else return RedirectToAction("Cesta", "ProcesarCompra");
        }
        public IActionResult EliminarDireccion(int id)
        {
            personaRepositorio.eliminarDireccion(id);
            return RedirectToAction("Index", "Persona");
        }
        [HttpGet]
        public IActionResult ActualizarContraseña()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ActualizarContraseña(string contraseñaActual, string contraseñaNueva, string contraseñaVerificar)
        {
            int Id = GetLoggedUser().Id;
            contraseñaActual = Convertirsha256(contraseñaActual);
            if (_dbEntities.usuarios.Any(x => x.Id == Id && x.Contraseña == contraseñaActual))
            {
                if (contraseñaNueva == contraseñaVerificar)
                {
                    contraseñaNueva = Convertirsha256(contraseñaNueva);
                    personaRepositorio.actualizarContraseña(Id, contraseñaNueva);
                    ModelState.AddModelError("Actualizar", "Contraseña actualizada");
                    var persona = personaRepositorio.obtenerUsuarioId(Id);
                    ViewBag.Direcciones = personaRepositorio.obtenerDirecciones(Id);
                    return View("Index", persona);
                }
                ModelState.AddModelError("contraseñaNueva", "Las contraseñas no coinciden");
                return View();
            }
            ModelState.AddModelError("contraseñaActual", "Contraseña Erronea");
            return View();
        }
        private Usuario GetLoggedUser()
        {
            var claim = HttpContext.User.Claims.First();
            string username = claim.Value;
            return personaRepositorio.obtenerUsuarioString(username);
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
    }
}
