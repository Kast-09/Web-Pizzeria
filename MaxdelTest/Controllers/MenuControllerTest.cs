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
    public class MenuControllerTest
    {
        [Test]
        public void IndexViewCase01()
        {
            var mockMenu = new Mock<IMenuRepositorio>();
            mockMenu.Setup(o => o.listarProductos()).Returns(new List<Productos>());

            var indexT = new MenuController(null, mockMenu.Object);
            var view = indexT.Index("");

            Assert.IsNotNull(view);
        }
    }
}
