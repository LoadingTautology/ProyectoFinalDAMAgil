using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDAMAgil.Services.Administrador;
using ProyectoFinalDAMAgil.Services.Centroeducativo;
using ProyectoFinalDAMAgil.Services.Correoelectronico;
using ProyectoFinalDAMAgil.Services.Usuario;
using ProyectoFinalDAMAgil.Services.Usuarioscentroeducativo;
using ProyectoFinalDAMAgil.Models.Admin;
using System.Security.Claims;

namespace ProyectoFinalDAMAgil.Controllers
{
    [Authorize (Roles = "ADMINISTRADOR")]
    public class AdminController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ICorreoelectronicoService _correoelectronicoService;
        private readonly IAdministradorService _administradorService;
        private readonly ICentroeducativo _centroeducativoService;
        private readonly IUsuarioscentroeducativo _usuarioscentroeducativoService;

        public AdminController(IUsuarioService usuarioService,
                               ICorreoelectronicoService correoelectronicoService,
                               IAdministradorService administradorService,
                               ICentroeducativo centroeducativoService,
                               IUsuarioscentroeducativo usuarioscentroeducativo)
        {
            _usuarioService=usuarioService;
            _correoelectronicoService=correoelectronicoService;
            _administradorService=administradorService;
            _centroeducativoService=centroeducativoService;
            this._usuarioscentroeducativoService=usuarioscentroeducativo;
        }



        #region Gestion Centro
        [HttpGet]
        public async Task<IActionResult> ListarCentroAsync()
        {
            ClaimsPrincipal claimsUser = HttpContext.User;
            string emailUsuario = "";
            if (claimsUser.Identity!.IsAuthenticated)
                emailUsuario = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault()!;
            

            IEnumerable<CentroEducativoModel> listado = await _centroeducativoService.ListadoCentroEducativo(emailUsuario);

            Console.WriteLine("AQUI LLEGA LISTAR CENTRO");
            return View("~/Views/Admin/Centro/Listar.cshtml", listado);
        }


        [HttpGet]
        public IActionResult GuardarCentro()
        {
            return View("~/Views/Admin/Centro/Guardar.cshtml");
        }



        [HttpPost]
        public async Task<IActionResult> GuardarCentro(CentroEducativoModel datosCentro)
        {

            if (!ModelState.IsValid)
                return View("~/Views/Admin/Centro/Guardar.cshtml", datosCentro);

            if(await _centroeducativoService.ExisteCentroEducativo(datosCentro.NombreCentro, datosCentro.DireccionCentro)) 
            {
                datosCentro.NombreCentro="";
                datosCentro.DireccionCentro="";
                ViewData["Mensaje"] = "Ese centro ya existe en esa dirección";
                return View("~/Views/Admin/Centro/Guardar.cshtml", datosCentro);
            }
            else 
            {
                try 
                {
                    //Crear Centro Educativo
                    Scaffold.Centroeducativo centroCreado = await _centroeducativoService.SaveCentroeducativo(
                        new Scaffold.Centroeducativo
                        {
                            NombreCentro=datosCentro.NombreCentro,
                            Direccion = datosCentro.DireccionCentro

                        }, HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault()!
                    );

                    //Crear Relacion Centro Usuarios
                    await _usuarioscentroeducativoService.SaveUsuariosCentroeducativo(
                        new Scaffold.Usuarioscentroeducativo 
                        {
                            IdCentro = centroCreado.IdCentro,
                            IdUsuario = centroCreado.IdAdministrador
                        }
                    );


                }
                catch (Exception ex) 
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            return RedirectToAction("ListarCentro", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> EditarCentro(CentroEducativoModel datosCentro)
        {
            Console.WriteLine("************************** EDITAR CENTRO"+ datosCentro);

            //METODO SOLO DEVUELVE LA VISTA
            return View("~/Views/Admin/Centro/Editar.cshtml", datosCentro);
        }

        #endregion

        #region Gestion Profesores

        #endregion

        #region Gestion Alumnos

        #endregion

        public IActionResult GestionCentros()
        {
            return View();
        }

        public IActionResult Prueba()
        {
            Console.WriteLine("AQUI LLEGA");
            return View("~/Views/Admin/Carpeta/Index.cshtml");
        }
    }
}

/*





        public IActionResult Editar(int IdContacto)
        {
            //METODO SOLO DEVUELVE LA VISTA
            var ocontacto = _ContactoDatos.Obtener(IdContacto);
            return View(ocontacto);
        }

        [HttpPost]
        public IActionResult Editar(ContactoModel oContacto)
        {
            if (!ModelState.IsValid)
                return View();


            var respuesta = _ContactoDatos.Editar(oContacto);

            if (respuesta)
                return RedirectToAction("Listar");
            else
                return View();
        }


        public IActionResult Eliminar(int IdContacto)
        {
            //METODO SOLO DEVUELVE LA VISTA
            var ocontacto = _ContactoDatos.Obtener(IdContacto);
            return View(ocontacto);
        }

        [HttpPost]
        public IActionResult Eliminar(ContactoModel oContacto)
        {
  
            var respuesta = _ContactoDatos.Eliminar(oContacto.IdContacto);

            if (respuesta)
                return RedirectToAction("Listar");
            else
                return View();
        }
 */
