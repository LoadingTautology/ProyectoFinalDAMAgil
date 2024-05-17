using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDAMAgil.Services.Administrador;
using ProyectoFinalDAMAgil.Services.Centroeducativo;
using ProyectoFinalDAMAgil.Services.Correoelectronico;
using ProyectoFinalDAMAgil.Services.Usuario;
using ProyectoFinalDAMAgil.Services.Usuarioscentroeducativo;
using ProyectoFinalDAMAgil.Models.Admin;
using System.Security.Claims;
using ProyectoFinalDAMAgil.Scaffold;

namespace ProyectoFinalDAMAgil.Controllers
{
    [Authorize (Roles = "ADMINISTRADOR")]
    public class AdminController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ICorreoelectronicoService _correoelectronicoService;
        private readonly IAdministradorService _administradorService;
        private readonly ICentroeducativoService _centroeducativoService;
        private readonly IUsuarioscentroeducativoService _usuarioscentroeducativoService;

        public AdminController(IUsuarioService usuarioService,
                               ICorreoelectronicoService correoelectronicoService,
                               IAdministradorService administradorService,
                               ICentroeducativoService centroeducativoService,
                               IUsuarioscentroeducativoService usuarioscentroeducativo)
        {
            _usuarioService=usuarioService;
            _correoelectronicoService=correoelectronicoService;
            _administradorService=administradorService;
            _centroeducativoService=centroeducativoService;
            this._usuarioscentroeducativoService=usuarioscentroeducativo;
        }



        #region Gestion Centro
        [HttpGet]
        public async Task<IActionResult> ListarCentro()
        {
            ClaimsPrincipal claimsUser = HttpContext.User;
            string emailUsuario = "";
            if (claimsUser.Identity!.IsAuthenticated)
                emailUsuario = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault()!;
            
            /*            */
            IEnumerable<Scaffold.Centroeducativo> listado = await _centroeducativoService.ListadoCentroEducativo(emailUsuario);

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
        public async Task<IActionResult> EditarCentro([FromRoute] int Id)
        {

            Scaffold.Centroeducativo centro = await _centroeducativoService.GetCentroeducativo(Id);

            return View("~/Views/Admin/Centro/Editar.cshtml", centro);
        }


        [HttpPost]
        public async Task<IActionResult> EditarCentro(Scaffold.Centroeducativo centro)
        {

            if(await _centroeducativoService.ExisteCentroEducativo(centro.NombreCentro, centro.Direccion)) 
            {
                ViewData["Mensaje"] = "Ese centro ya existe en esa dirección";
                return View("~/Views/Admin/Centro/Editar.cshtml", centro);
            }

            await _centroeducativoService.UpdateCentroeducativo(centro);

            return RedirectToAction("ListarCentro");
            //return View("~/Views/Admin/Centro/Editar.cshtml", datosCentro);
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> EliminarCentro([FromRoute] int Id)
        {

            Scaffold.Centroeducativo centro = await _centroeducativoService.DeleteCentroeducativo(Id);
            if (await _centroeducativoService.ExisteCentroEducativo(centro.NombreCentro, centro.Direccion))
                return RedirectToAction("ListarCentro");
            else
                return RedirectToAction("ListarCentro");
        }

        #endregion

        #region Gestion Profesores

        #endregion

        #region Gestion Alumnos

        #endregion


    }
}