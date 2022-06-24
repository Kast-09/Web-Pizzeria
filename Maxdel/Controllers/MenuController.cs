using Maxdel.DB;
using Maxdel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Maxdel.Models;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using Maxdel.Repositorio;

namespace Maxdel.Controllers
{
    public class MenuController : Controller
    {
        private readonly DbEntities _dbEntities;
        private readonly IMenuRepositorio menuRepositorio;

        public MenuController(DbEntities dbEntities, IMenuRepositorio menuRepositorio)
        {
            _dbEntities = dbEntities;
            this.menuRepositorio = menuRepositorio;
        }

        public IActionResult Index(string buscar)
        {
            var ListaProductos = menuRepositorio.listarProductos();
            if (buscar != null && buscar != "")
            {
                ListaProductos = ListaProductos.Where(o => o.Nombre.Contains(buscar, StringComparison.OrdinalIgnoreCase) || o.Descripcion.Contains(buscar, StringComparison.OrdinalIgnoreCase)).OrderBy(o => o.Nombre).ToList();
            }
            ViewBag.Buscar = buscar;
            return View(ListaProductos);
        }
    }
}
