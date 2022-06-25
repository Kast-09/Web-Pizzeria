using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maxdel.Controllers;
using Moq;
using NUnit.Framework;
using Maxdel.Repositorio;
using Maxdel.Models;
using Microsoft.AspNetCore.Mvc;

namespace MaxdelTest.Controllers
{
    public class ProductosControllerTest
    {
        [Test]
        public void EditarPostViewCase01()
        {
            var mockEditar = new Mock<IProductosRepositorios>();
            mockEditar.Setup(o => o.editarProducto(1, new Productos()));

            var editarT = new ProductosController(null, mockEditar.Object);
            var view = editarT.EditarProducto(1, new Productos());

            Assert.IsNotNull(view);
            Assert.IsInstanceOf<RedirectToActionResult>(view);
        }

        [Test]
        public void AgregarPostViewCase01()
        {
            var mockEditar = new Mock<IProductosRepositorios>();
            mockEditar.Setup(o => o.AgregarTamañoPrecio(1, new TamañoPrecio()));

            var agregarT = new ProductosController(null, mockEditar.Object);
            var view = agregarT.AgregarTamañoPrecio(1, new TamañoPrecio());

            Assert.IsNotNull(view);
        }

        [Test]
        public void EditarTamañoPostViewCase01()
        {
            var mockEditar = new Mock<IProductosRepositorios>();
            mockEditar.Setup(o => o.editarTamaño(1, "Personal", 15));

            var editarT = new ProductosController(null, mockEditar.Object);
            var view = editarT.EditarTamañoPrecio(1, new TamañoPrecio());

            Assert.IsNotNull(view);
            Assert.IsInstanceOf<RedirectToActionResult>(view);
        }

        [Test]
        public void EliminarTamañoViewCase01()
        {
            var mockEdliminar = new Mock<IProductosRepositorios>();
            mockEdliminar.Setup(o => o.eliminarTamañoPrecio(1));

            var eliminarT = new ProductosController(null, mockEdliminar.Object);
            var view = eliminarT.EliminarTamañoPrecio(1);

            Assert.IsNotNull(view);
            Assert.IsInstanceOf<RedirectToActionResult>(view);
        }
    }
}
