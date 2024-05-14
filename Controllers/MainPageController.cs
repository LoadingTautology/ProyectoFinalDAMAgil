using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinalDAMAgil.Controllers
{
    public class MainPageController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}
