﻿using Maxdel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Maxdel.DB;
using Microsoft.EntityFrameworkCore;
using Maxdel.Repositorio;

namespace Maxdel.Controllers
{
    public class HomeController : Controller
    {
        private DbEntities _dbEntities;
        private readonly IHomeRepositorio _homeRepositorio;

        public HomeController(DbEntities dbEntities, IHomeRepositorio homeRepositorio)
        {
            _dbEntities = dbEntities;
            _homeRepositorio = homeRepositorio;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Menu()
        {
            ViewBag.ListaProductos = _homeRepositorio.ObtenerProductos();
            return View();
        }
        public IActionResult Detalle( int IdProducto)
        {
            ViewBag.Producto = _dbEntities.Productos.Where(o => o.Id == IdProducto).ToList();
            ViewBag.Tamanios = _dbEntities.tamaños.Where(o => o.IdProducto == IdProducto).ToList();
            ViewBag.Precios = _dbEntities.precios.Where(o => o.IdProducto == IdProducto).ToList();
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Cesta(int Id)
        {
            Id = 1;
            ViewBag.DatosCesta = _homeRepositorio.ObtenerDetalleCesta(Id);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}