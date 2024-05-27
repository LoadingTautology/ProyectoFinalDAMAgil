using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinalDAMAgil.Controllers
{
    public class pfcController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Grantt()
        {
            return View("~/Views/pfc/Grantt.cshtml");
        }
    }
}
