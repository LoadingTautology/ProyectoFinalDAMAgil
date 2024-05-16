using Microsoft.AspNetCore.Mvc;

namespace ProyectoFinalDAMAgil.Controllers
{
    public class MainPageController : Controller
    {
        #region Index
        public IActionResult Index()
        {
            if (HttpContext.User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        #endregion
        #region AboutUs
        public IActionResult AboutUs()
        {
            if (HttpContext.User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        #endregion
        #region Contacto
        public IActionResult Contacto()
        {
            if (HttpContext.User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        #endregion
        #region Privacy
        public IActionResult Privacy()
        {
            if (HttpContext.User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        #endregion
    }
}
