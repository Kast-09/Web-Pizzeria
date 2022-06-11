using Microsoft.AspNetCore.Mvc;
using Maxdel.Models;
using System.Diagnostics;
using Maxdel.DB;
using Microsoft.AspNetCore.Authorization;

namespace Maxdel.Controllers
{
    [Authorize]
    public class PersonaController : Controller
    {
        DbEntities _dbEntities;
        public PersonaController(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }

        public IActionResult Index()
        {
            int Id = GetLoggedUser().Id;
            var persona = _dbEntities.usuarios.First(o => o.Id == Id);
            ViewBag.Direcciones = _dbEntities.direcciones.Where(o => o.IdUsuario == Id).ToList();
            return View(persona);
        }
        [HttpGet]
        public IActionResult EditarPersona()
        {
            int id = GetLoggedUser().Id;
            var persona = _dbEntities.usuarios.First(o => o.Id == id); 
            return View(persona);
        }
        [HttpPost]
        public IActionResult EditarPersona(int id, Usuario user)
        {
            var Persona = _dbEntities.usuarios.First(o => o.Id == id);
            Persona.Nombre = user.Nombre;
            Persona.Apellido = user.Apellido;
            Persona.NroCelular = user.NroCelular;
            Persona.Correo = user.Correo;
            _dbEntities.SaveChanges();
            return RedirectToAction("Index", "Persona");
        }
        public IActionResult AgregaDireccion()
        {
            return View();
        }
        public IActionResult NuevaDireccion(int retorno, string Direccion, string Referencia)
        {
            int Id = GetLoggedUser().Id;
            Direcciones direccion = new Direcciones();
            direccion.IdUsuario = Id;
            direccion.Direccion = Direccion;
            direccion.Referencia = Referencia;
            _dbEntities.direcciones.Add(direccion);
            _dbEntities.SaveChanges();
            if(retorno == 1) return RedirectToAction("Index", "Persona");
            else return RedirectToAction("Cesta", "ProcesarCompra");
        }
        public IActionResult EliminarDireccion(int id)
        {
            var direccion = _dbEntities.direcciones.First(o => o.Id == id);
            _dbEntities.direcciones.Remove(direccion);
            _dbEntities.SaveChanges();
            return RedirectToAction("Index", "Persona");
        }
        private Usuario GetLoggedUser()
        {
            var claim = HttpContext.User.Claims.First();
            string username = claim.Value;
            return _dbEntities.usuarios.First(o => o.Correo == username);
        }
    }
}
