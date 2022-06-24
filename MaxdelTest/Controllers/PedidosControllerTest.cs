using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Maxdel.Repositorio;
using Maxdel.Models;
using Maxdel.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace MaxdelTest.Controllers
{
    public class PedidosControllerTest
    {
        [Test]
        public void ActualizarViewCase()
        {
            var mockPedidos = new Mock<IPedidosRepositorio>();
            mockPedidos.Setup(o => o.listarPedidos(1)).Returns(new List<Pedido>());

            var actualizarT = new PedidosController(null, mockPedidos.Object);
            var view = actualizarT.ActualizarEstado(1, 2, 1);

            Assert.IsNotNull(view);
            Assert.IsInstanceOf<RedirectToActionResult>(view);
        }
    }
}
