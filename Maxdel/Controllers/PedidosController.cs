﻿using Maxdel.DB;
using Maxdel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Maxdel.Controllers
{
    [Authorize]
    public class PedidosController : Controller
    {
        private readonly DbEntities _dbEntities;

        public PedidosController(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
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
            ViewBag.Boletas = _dbEntities.boletas
                                .Include("Pedidos")
                                .Include("Pedidos.EstadoFK")
                                .Include("Pedidos.DetallePedidos")
                                .Include("Pedidos.DetallePedidos.Producto")
                                .Where(o => o.Pedidos.Any(x => x.Estado >= 2) && o.Pedidos.Any(x => x.Estado <= 4)).ToList();

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
            ViewBag.Boletas = _dbEntities.boletas
                                .Include("Pedidos")
                                .Include("Pedidos.EstadoFK")
                                .Include("Pedidos.DetallePedidos")
                                .Include("Pedidos.DetallePedidos.Producto")
                                .Where(o => o.Pedidos.Any(x => x.Estado == 5)).ToList();

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
            ViewBag.Boletas = _dbEntities.boletas
                                .Include("Pedidos")
                                .Include("Pedidos.EstadoFK")
                                .Include("Pedidos.DetallePedidos")
                                .Include("Pedidos.DetallePedidos.Producto")
                                .Where(o => o.Pedidos.Any(x => x.Estado == 6)).ToList();

            ViewBag.Estados = _dbEntities.estado.ToList();

            ViewBag.Apartado = "Anulados: ";

            ViewBag.Retorno = 3;

            return View("Lista");
        }

        public IActionResult ActualizarEstado(int id, int estado, int retorno)
        {
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
            if (id > 6 || id < 1)
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
            var pedidos = _dbEntities.pedidos.Where(o => o.IdBoleta == id).ToList();

            for(int i = 0; i < pedidos.Count; i++)
            {
                pedidos[i].Estado = estado;
            }
            _dbEntities.SaveChanges();

            if(retorno == 1)
            {
                return RedirectToAction("Espera");
            }
            else if(retorno == 2)
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
            return _dbEntities.usuarios.First(o => o.Correo == username);
        }
    }
}
