using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maxdel.DB;
using Maxdel.Models;
using Moq;
using NUnit.Framework;
using Maxdel.Controllers;
using Microsoft.AspNetCore.Mvc;
using Maxdel.Repositorio;

namespace MaxdelTest.Controllers
{

    public class HomeControllerTest
    {
        private readonly IHomeRepositorio _homeRepositorio;

        //[Test]
        //public void IndexViewCase01()
        //{
        //    var mockController = new Mock<IHomeRepositorio>();

        //    var controller = new HomeController(null, mockController.Object);
        //    var view = controller.Index();

        //    Assert.IsNotNull(view);
        //}

        //[Test]
        //public void CestaView()
        //{
        //    var mockDetalleRepo = new Mock<IHomeRepositorio>();
        //    mockDetalleRepo.Setup(o => o.ObtenerDetalleCesta(1)).Returns(new List<DetallePedido>());

        //    var controller = new HomeController(null, mockDetalleRepo.Object);
        //    var ResultadoC = controller.Cesta(1);
        //    Assert.IsNotNull(ResultadoC);
        //}

        //[Test]
        //public void AboutViewCase01()
        //{
        //    var mockController = new Mock<IHomeRepositorio>();

        //    var controller = new HomeController(null, mockController.Object);
        //    var view = controller.About();

        //    Assert.IsNotNull(view);
        //}

        //[Test]
        //public void MenuViewCase01()
        //{
        //    var mockController = new Mock<IHomeRepositorio>();
        //    var controller = new HomeController(null, mockController.Object);
        //    var view = controller.Menu() as ViewResult;

        //    Assert.IsNotNull(view);
        //}
    }
}
