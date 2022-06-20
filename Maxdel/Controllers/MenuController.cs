using Maxdel.DB;
using Maxdel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Maxdel.Models;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Maxdel.Controllers
{
    public class MenuController : Controller
    {
        private readonly DbEntities _dbEntities;

        public MenuController(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }

        public IActionResult Index(string buscar)
        {
            //string aux = Regex.Replace(buscar.Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "");
            var ListaProductos = _dbEntities.Productos.ToList();
            if (buscar != null && buscar != "")
            {
                ListaProductos = ListaProductos.Where(o => o.Nombre.Contains(buscar, StringComparison.OrdinalIgnoreCase) || o.Descripcion.Contains(buscar, StringComparison.OrdinalIgnoreCase)).OrderBy(o => o.Nombre).ToList();
            }
            ViewBag.Buscar = buscar;
            return View(ListaProductos);
        }
    }
}
