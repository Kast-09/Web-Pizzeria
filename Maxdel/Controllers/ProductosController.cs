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
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
            var ListaProductos = _dbEntities.Productos
                                        .Include("TamañoPrecios")
                                        .ToList();
            return View(ListaProductos);
        }

        [HttpGet]
        public IActionResult AgregarProducto()
        {
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
            return View();
        }
        [HttpPost]
        public IActionResult AgregarProducto(Productos productos)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("AgregarProducto", "Rellene los datos");
                return View("AgregarProducto");
            }
            _dbEntities.Productos.Add(productos);
            _dbEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditarProducto(int Id)
        {
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
            var Producto = _dbEntities.Productos.First(o => o.Id == Id);
            return View(Producto);
        }
        [HttpPost]
        public IActionResult EditarProducto(int Id, Productos productos)
        {
            if (!ModelState.IsValid)
            {
                var Producto = _dbEntities.Productos.First(o => o.Id == Id);
                ModelState.AddModelError("EditarProducto", "Rellene los datos");
                return View("EditarProducto", Producto);
            }
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
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
            var Producto = _dbEntities.Productos.First(o => o.Id == Id);
            return View(Producto);
        }
        [HttpPost]
        public IActionResult AgregarTamañoPrecio(int Id, TamañoPrecio tamañoPrecio)
        {
            if (!ModelState.IsValid)
            {
                var Producto = _dbEntities.Productos.First(o => o.Id == Id);
                ModelState.AddModelError("Agregar", "Rellene los datos");
                return View("AgregarTamañoPrecio", Producto);
            }
            if (tamañoPrecio.Precio <= 0)
            {
                var Producto = _dbEntities.Productos.First(o => o.Id == Id);
                ModelState.AddModelError("Agregar", "Rellene los datos");
                return View("AgregarTamañoPrecio", Producto);
            }
            TamañoPrecio tamañoPrecio1 = new TamañoPrecio();
            tamañoPrecio1.IdProducto = Id;
            tamañoPrecio1.TamañoProducto = tamañoPrecio.TamañoProducto;
            tamañoPrecio1.Precio = tamañoPrecio.Precio;
            _dbEntities.tamañoPrecios.Add(tamañoPrecio1);
            _dbEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditarTamañoPrecio(int Id)
        {
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
            TamañoPrecio tamañoPrecio = _dbEntities.tamañoPrecios.First(o => o.Id == Id);
            ViewBag.Producto = _dbEntities.Productos.First(o => o.Id == tamañoPrecio.IdProducto);
            return View(tamañoPrecio);
        }
        [HttpPost]
        public IActionResult EditarTamañoPrecio(int Id, TamañoPrecio tamañoPrecio)
        {
            if (!ModelState.IsValid)
            {
                TamañoPrecio tamañoPrecioo = _dbEntities.tamañoPrecios.First(o => o.Id == Id);
                ViewBag.Producto = _dbEntities.Productos.First(o => o.Id == tamañoPrecioo.IdProducto);
                return View("EditarTamañoPrecio", tamañoPrecio);
            }
            if (tamañoPrecio.Precio < 0)
            {
                TamañoPrecio tamañoPrecioo = _dbEntities.tamañoPrecios.First(o => o.Id == Id);
                ViewBag.Producto = _dbEntities.Productos.First(o => o.Id == tamañoPrecioo.IdProducto);
                return View("EditarTamañoPrecio", tamañoPrecio);
            }
            TamañoPrecio tamañoPrecio1 = _dbEntities.tamañoPrecios.First(o => o.Id == Id);
            tamañoPrecio1.TamañoProducto = tamañoPrecio.TamañoProducto;
            tamañoPrecio1.Precio = tamañoPrecio.Precio;
            _dbEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult EliminarTamañoPrecio(int Id)
        {
            int IdRol = GetLoggedUser().IdRol;
            if (IdRol != 1)
            {
                return RedirectToAction("Index", "Excepcion");
            }
            var precioTam = _dbEntities.tamañoPrecios.First(o => o.Id == Id);
            _dbEntities.tamañoPrecios.Remove(precioTam);
            _dbEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        private Usuario GetLoggedUser()
        {
            var claim = HttpContext.User.Claims.First();
            string username = claim.Value;
            return _dbEntities.usuarios.First(o => o.Correo == username);
        }
    }
}
