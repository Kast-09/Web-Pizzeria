using Maxdel.DB;
using Maxdel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Maxdel.Models;

namespace Maxdel.Controllers
{
    public class MenuController : Controller
    {
        private DbEntities _dbEntities;

        public MenuController(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }

        public IActionResult Index()
        {
             var ListaProductos = _dbEntities.Productos.ToList();
            return View(ListaProductos);
        }
    }
}
