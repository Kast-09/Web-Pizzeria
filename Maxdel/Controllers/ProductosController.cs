﻿using Maxdel.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Maxdel.Models;

namespace Maxdel.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly DbEntities _dbEntities;

        public ProductosController(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }

        public IActionResult Index()
        {
            var ListaProductos = _dbEntities.Productos.ToList();
            ViewBag.Tamaños = _dbEntities.tamañoPrecios.ToList();
            return View(ListaProductos);
        }

        [HttpGet]
        public IActionResult AgregarProducto()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AgregarProducto(Productos productos)
        {
            _dbEntities.Productos.Add(productos);
            _dbEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditarProducto(int Id)
        {
            var Producto = _dbEntities.Productos.First(o => o.Id == Id);
            return View(Producto);
        }
        [HttpPost]
        public IActionResult EditarProducto(int Id, Productos productos)
        {
            var producto = _dbEntities.Productos.First(o => o.Id == Id);
            producto.Nombre = productos.Nombre;
            producto.Descripcion = productos.Descripcion;
            producto.UrlImagen = productos.UrlImagen;
            _dbEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AgregarTamañoPrecio(int Id)
        {
            var Producto = _dbEntities.Productos.First(o => o.Id == Id);
            return View(Producto);
        }
        [HttpPost]
        public IActionResult AgregarTamañoPrecio(int Id, TamañoPrecio tamañoPrecio)
        {
            TamañoPrecio tamañoPrecio1 = new TamañoPrecio();
            tamañoPrecio1.IdProducto = Id;
            tamañoPrecio1.TamañoProducto = tamañoPrecio.TamañoProducto;
            tamañoPrecio1.Precio = tamañoPrecio.Precio;
            _dbEntities.tamañoPrecios.Add(tamañoPrecio1);
            _dbEntities.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
