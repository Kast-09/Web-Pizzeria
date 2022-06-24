using Microsoft.AspNetCore.Mvc;

namespace Maxdel.Controllers
{
    public class ExcepcionController : Controller
    {
        public ExcepcionController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
