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

        [Test]
        public void IndexViewCase01()
        {
            var indexT = new HomeController();
            var view = indexT.Index();

            Assert.IsNotNull(view);
        }

        [Test]
        public void AbuotViewCase01()
        {
            var aboutT = new HomeController();
            var view = aboutT.About();

            Assert.IsNotNull(view);
        }
    }
}
