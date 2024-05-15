using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDAMAgil.Scaffold;
using ProyectoFinalDAMAgil.Services.Usuario;
using ProyectoFinalDAMAgil.Utilities;
using System.Security.Claims;
using ProyectoFinalDAMAgil.Services.Correoelectronico;
using ProyectoFinalDAMAgil.Models;
using ProyectoFinalDAMAgil.Services.Administrador;

namespace ProyectoFinalDAMAgil.Controllers
{
    public class LoginController : Controller
    {
        //Inyeccion de dependencias que hay en Program.cs
        private readonly IUsuarioService _usuarioService;
        private readonly ICorreoelectronicoService _correoelectronicoService;
        private readonly IAdministradorService _administradorService;


        public LoginController(IUsuarioService usuarioService, ICorreoelectronicoService correoelectronicoService, IAdministradorService administradorService)
        {
            _usuarioService = usuarioService;
            _correoelectronicoService =correoelectronicoService;
            _administradorService = administradorService;
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

            if (await _usuarioService.ExisteUsuario(usuarioView.Correo)) 
            {
                usuarioView.Correo ="";
                ViewData["Mensaje"] = "Ese email ya existe";
                return View(usuarioView);
            }
            else if(usuarioView.Clave != ClaveRepetida)
            {
                ViewData["Mensaje"] = "Clave repetida incorrecta";
                return View(usuarioView);
            }
            else
            {
                try
                {
                    //Crear Correo
                    Correoelectronico correoCreado = await _correoelectronicoService.SaveCorreoElectronico(
                        new Correoelectronico
                        {
                            Email = usuarioView.Correo,
                            Clave = Encryptor.EncriptarClave(usuarioView.Clave)
                        }
                    );
                    //Crear UsuarioAdmin
                    Usuario usuarioCreado = await _usuarioService.SaveUsuario(
                        new Usuario
                        {
                            NombreUsuario = usuarioView.NombreUsuario,
                            ApellidosUsuario = usuarioView.NombreUsuario,
                            Rol = "ADMINISTRADOR",
                            Email=usuarioView.Correo,
                        }
                    );

                    //Guardar Admin

                    Console.WriteLine("USUARIO CREADO ID: "+usuarioCreado.IdUsuario);
                    Console.WriteLine("USUARIO CREADO DNI: "+usuarioView.DNI);

                    Administrador administradorCreado = await _administradorService.SaveAdministrador(
                        new Administrador
                        {
                            IdAdministrador =usuarioCreado.IdUsuario,
                            Dni = usuarioView.DNI
                        }
                    );

                    return RedirectToAction("IniciarSesion", "Login");
                }
                catch (Exception e) 
                {
                    ViewData["Mensaje"] = "No se pudo crear el usuario";
                    return View();
                }
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

            List<Claim> claims = new List<Claim>() 
            {
                new Claim(ClaimTypes.Name, usuarioEncontrado.NombreUsuario), 
                new Claim(ClaimTypes.Role, usuarioEncontrado.Rol)  
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties() { AllowRefresh = true };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

            return RedirectToAction("Index", "Home");

        }
    }
}
