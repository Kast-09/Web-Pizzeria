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
    public class PersonaControllerTest
    {
        [Test]
        public void EliminarDireccionViewCase()
        {
            var mockPersona = new Mock<IPersonaRepositorio>();
            mockPersona.Setup(o => o.eliminarDireccion(1));

            var eliminarT = new PersonaController(null, mockPersona.Object);
            var view = eliminarT.EliminarDireccion(1);

            Assert.IsNotNull(view);
            Assert.IsInstanceOf<RedirectToActionResult>(view);
        }

        [Test]
        public void AgregarContraseñaViewCase()
        {
            var agregarT = new PersonaController(null, null);
            var view = agregarT.ActualizarContraseña();

            Assert.IsNotNull(view);
        }
    }
}
