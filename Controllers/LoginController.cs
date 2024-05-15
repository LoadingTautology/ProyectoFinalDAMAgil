using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDAMAgil.Scaffold;
using ProyectoFinalDAMAgil.Services.Usuario;
using ProyectoFinalDAMAgil.Utilities;
using System.Security.Claims;
using ProyectoFinalDAMAgil.Services.Correoelectronico;
using ProyectoFinalDAMAgil.Models;

namespace ProyectoFinalDAMAgil.Controllers
{
    public class LoginController : Controller
    {
        //Inyeccion de dependencias que hay en Program.cs
        private readonly IUsuarioService _usuarioService;
        private readonly ICorreoelectronicoService _correoelectronicoService;


        public LoginController(IUsuarioService usuarioService, ICorreoelectronicoService correoelectronicoService)
        {
            _usuarioService = usuarioService;
            _correoelectronicoService =correoelectronicoService;
        }

        [HttpGet]
        public IActionResult Registro()
        {
            if (HttpContext.User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(UsuarioCorreo usuarioView, string ClaveRepetida)
        {

            if(usuarioView.Clave != ClaveRepetida)
            {
                ViewData["Mensaje"] = "Clave repetida incorrecta";
                return View(usuarioView);
            }


            Correoelectronico correoCreado = await _correoelectronicoService.SaveCorreoElectronico(
                new Correoelectronico
                {
                    Email = usuarioView.Correo,
                    Clave = Encryptor.EncriptarClave(usuarioView.Clave)
                }
            );



            Usuario usuarioCreado = await _usuarioService.SaveUsuario(
                new Usuario
                {
                    NombreUsuario = usuarioView.NombreUsuario,
                    ApellidosUsuario = usuarioView.NombreUsuario,
                    Rol = "Administrador",
                    Email=usuarioView.Correo
                }
            ); ;

            if (usuarioCreado.IdUsuario > 0 && correoCreado.Email!=null)
            {
                return RedirectToAction("IniciarSesion", "Login");
            }
            else
            {
                ViewData["Mensaje"] = "No se pudo crear el usuario";
                return View();
            }

        }

        [HttpGet]
        public IActionResult IniciarSesion()
        {
            if (HttpContext.User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string correo, string clave)
        {
            Correoelectronico correoEncontrado = await _correoelectronicoService.GetCorreoElectronico(correo, Encryptor.EncriptarClave(clave));

            if (correoEncontrado == null)
            {
                ViewData["Mensaje"] = "Usuaio no encontrado";
                return View();
            }

            Usuario usuarioEncontrado = await _usuarioService.GetUsuario(correo);

            List<Claim> claims = new List<Claim>() { new Claim(ClaimTypes.Name, usuarioEncontrado.NombreUsuario) };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties() { AllowRefresh = true };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

            return RedirectToAction("Index", "Home");

        }
    }
}
