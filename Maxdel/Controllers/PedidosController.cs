using Maxdel.DB;
using Maxdel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Maxdel.Repositorio;

namespace Maxdel.Controllers
{
    [Authorize]
    public class PedidosController : Controller
    {
        private readonly DbEntities _dbEntities;
        private readonly IPedidosRepositorio pedidosRepositorio;

        public PedidosController(DbEntities dbEntities, IPedidosRepositorio pedidosRepositorio)
        {
            _dbEntities = dbEntities;
            this.pedidosRepositorio = pedidosRepositorio;
        }

        public IActionResult Index()
        {
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
            return View();
        }

        public IActionResult Espera()
        {
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
            ViewBag.Boletas = pedidosRepositorio.listarEspera();

            ViewBag.Estados = _dbEntities.estado.ToList();

            ViewBag.Apartado = "en Espera: ";

            ViewBag.Retorno = 1;

            return View("Lista");
        }

        public IActionResult Entregado()
        {
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
            ViewBag.Boletas = pedidosRepositorio.listarEntregado();

            ViewBag.Estados = _dbEntities.estado.ToList();

            ViewBag.Apartado = "Entregados: ";

            ViewBag.Retorno = 2;

            return View("Lista");
        }

        public IActionResult Anulado()
        {
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
            ViewBag.Boletas = pedidosRepositorio.listarAnulado();

            ViewBag.Estados = _dbEntities.estado.ToList();

            ViewBag.Apartado = "Anulados: ";

            ViewBag.Retorno = 3;

            return View("Lista");
        }

        public IActionResult ActualizarEstado(int id, int estado, int retorno)
        {
            if (estado > 6 || estado < 1)
            {
                ModelState.AddModelError("Estado", "Estado Erroneo");
                if (retorno == 1)
                {
                    return RedirectToAction("Espera");
                }
                else if (retorno == 2)
                {
                    return RedirectToAction("Entregado");
                }
                else
                {
                    return RedirectToAction("Anulado");
                }
            }
            var pedidos = pedidosRepositorio.listarPedidos(id);

            for (int i = 0; i < pedidos.Count; i++)
            {
                pedidos[i].Estado = estado;
            }
            pedidosRepositorio.actualizarEstado();
            if (retorno == 1)
            {
                return RedirectToAction("Espera");
            }
            else if (retorno == 2)
            {
                return RedirectToAction("Entregado");
            }
            else
            {
                return RedirectToAction("Anulado");
            }
        }

        private Usuario GetLoggedUser()
        {
            var claim = HttpContext.User.Claims.First();
            string username = claim.Value;
            return pedidosRepositorio.obtenerUsuario(username);
        }
    }
}
