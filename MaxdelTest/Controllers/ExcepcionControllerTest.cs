using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maxdel.Controllers;

namespace MaxdelTest.Controllers
{
    public class ExcepcionControllerTest
    {
        [Test]
        public void IndexViewCase01()
        {
            var indexT = new ExcepcionController();
            var view = indexT.Index();

            Assert.IsNotNull(view);
        }
    }
}
