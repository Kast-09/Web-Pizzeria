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
    public class AuthControllerTest
    {
        [Test]
        public void LoginGetViewCase01()
        {
            var indexT = new AuthController(null, null);
            var view = indexT.Login();

            Assert.IsNotNull(view);
        }

        [Test]
        public void RegisterGetViewCase01()
        {
            var mockRegister = new Mock<IAuthRepositorio>();
            mockRegister.Setup(o => o.obtenerPreguntas()).Returns(new List<PreguntaSeguridad>());

            var registerT = new AuthController(null, mockRegister.Object);
            var view = registerT.Register();

            Assert.IsNotNull(view);
        }
        [Test]
        public void CorreoGetViewCase01()
        {
            var correoT = new AuthController(null, null);
            var view = correoT.Correo();

            Assert.IsNotNull(view);
        }
    }
}
