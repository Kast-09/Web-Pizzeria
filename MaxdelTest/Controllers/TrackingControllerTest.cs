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

namespace MaxdelTest.Controllers
{
    public class TrackingControllerTest
    {
        [Test]
        public void IndexViewCase01()
        {
            var indexT = new TrackingController(null, null);
            var view = indexT.Index();

            Assert.IsNotNull(view);
        }

        //[Test]
        //public void EstadoViewCase01()
        //{
        //    var mockRegister = new Mock<ITrackingRepositorio>();
        //    mockRegister.Setup(o => o.obtenerEstado("MAXKS000052")).Returns(new DetallePedido());

        //    var estadoT = new TrackingController(null, mockRegister.Object);
        //    var view = estadoT.Estado("MAXKS000052");

        //    Assert.IsNotNull(view);
        //}
    }
}
