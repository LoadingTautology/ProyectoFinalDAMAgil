using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDAMAgil.Models;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoFinalDAMAgil.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

        [HttpGet]
        public IActionResult Index()
		{
			ClaimsPrincipal claimsUser = HttpContext.User;
			string nombreUsuario = "";

			if (claimsUser.Identity!.IsAuthenticated)
			{
				nombreUsuario = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault()!;
			}

			ViewData["nombreUsuario"] = nombreUsuario;
			return View();
		}
		//[Authorize(Roles ="ADMINISTRADOR")]
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}


		public async Task<IActionResult> CerrarSesion()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "MainPage");
		}
	}
}
