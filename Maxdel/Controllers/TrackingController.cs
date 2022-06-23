using Microsoft.AspNetCore.Mvc;
using Maxdel.DB;
using Microsoft.EntityFrameworkCore;
using Maxdel.Models;

namespace Maxdel.Controllers
{
    public class TrackingController : Controller
    {
        DbEntities _dbEntities;

        public TrackingController(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Estado(string CodTracking)
        {
            if(CodTracking == null || CodTracking == "")
            {
                return View("Index");
            }
            var detallePedido = _dbEntities.detallePedidos
                                           .Include(o => o.Pedido)
                                           .Include(o => o.Producto)
                                           .FirstOrDefault(o => o.Pedido.CodTracking == CodTracking);
            ViewBag.CodTracking = CodTracking;
            if(detallePedido != null)
            {
                switch (detallePedido.Pedido.Estado)
                {
                    case 2: ViewBag.Estado = "Su pedido ha sido confirmado, en unos minutos tendra novedades."; break;
                    case 3: ViewBag.Estado = "Su pedido esta siendo preparado, en unos minutos tendra novedades."; break;
                    case 4: ViewBag.Estado = "Su pedido ha sido enviado, en unos minutos llegara a su domicilio."; break;
                    case 5: ViewBag.Estado = "Su pedido se ha entregado con éxito."; break;
                    case 6: ViewBag.Estado = "Su pedido se ha cancelado."; break;
                }
            }
            else
            {
                ViewBag.Estado = "Código de tracking invalido.";
            }
            return View();
        }
    }
}
