using Microsoft.AspNetCore.Mvc;
using Maxdel.DB;
using Microsoft.EntityFrameworkCore;
using Maxdel.Models;
using Maxdel.Repositorio;

namespace Maxdel.Controllers
{
    public class TrackingController : Controller
    {
        private readonly DbEntities _dbEntities;
        private readonly ITrackingRepositorio trackingRepositorio;

        public TrackingController(DbEntities dbEntities, ITrackingRepositorio trackingRepositorio)
        {
            _dbEntities = dbEntities;
            this.trackingRepositorio = trackingRepositorio;
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
            var detallePedido = trackingRepositorio.obtenerEstado(CodTracking);
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
