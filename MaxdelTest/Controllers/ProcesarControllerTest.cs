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
    public class ProcesarControllerTest
    {
        [Test]
        public void DetalleViewCase()
        {
            var mockDetalle = new Mock<IProcesarCompraRepositorio>();
            mockDetalle.Setup(o => o.obtenerProducto(1)).Returns(new Productos());

            var detalleT = new ProcesarCompraController(null, mockDetalle.Object);
            var view = detalleT.Detalle(1);

            Assert.IsNotNull(view);
        }

        [Test]
        public void ActualizarCantidadViewCase()
        {
            var mockDetalle = new Mock<IProcesarCompraRepositorio>();
            mockDetalle.Setup(o => o.actualizarCantidad(1, 1));

            var actualizarT = new ProcesarCompraController(null, mockDetalle.Object);
            var view = actualizarT.ActualizarCantidad(1, 1);

            Assert.IsNotNull(view);
        }

        [Test]
        public void DeleteViewCase()
        {
            var mockDetalle = new Mock<IProcesarCompraRepositorio>();
            mockDetalle.Setup(o => o.eliminarPedido(1));

            var deleteT = new ProcesarCompraController(null, mockDetalle.Object);
            var view = deleteT.Delete(1);

            Assert.IsNotNull(view);
            Assert.IsInstanceOf<RedirectToActionResult>(view);
        }
    }
}
