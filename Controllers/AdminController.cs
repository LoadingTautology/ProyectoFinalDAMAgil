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
using ProyectoFinalDAMAgil.Services.Cicloformativo;
using System.Runtime.InteropServices;

namespace ProyectoFinalDAMAgil.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR")]
    public class AdminController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ICorreoelectronicoService _correoelectronicoService;
        private readonly IAdministradorService _administradorService;
        private readonly ICentroeducativoService _centroeducativoService;
        private readonly IUsuarioscentroeducativoService _usuarioscentroeducativoService;
        private readonly ICicloformativoService _cicloformativoService;

        public AdminController(IUsuarioService usuarioService,
                               ICorreoelectronicoService correoelectronicoService,
                               IAdministradorService administradorService,
                               ICentroeducativoService centroeducativoService,
                               IUsuarioscentroeducativoService usuarioscentroeducativo,
                               ICicloformativoService cicloformativoService)
        {
            _usuarioService = usuarioService;
            _correoelectronicoService = correoelectronicoService;
            _administradorService = administradorService;
            _centroeducativoService = centroeducativoService;
            _usuarioscentroeducativoService = usuarioscentroeducativo;
            _cicloformativoService = cicloformativoService;

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

            if (await _centroeducativoService.ExisteCentroEducativo(datosCentro.NombreCentro, datosCentro.DireccionCentro))
            {
                datosCentro.NombreCentro = "";
                datosCentro.DireccionCentro = "";
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
                            NombreCentro = datosCentro.NombreCentro,
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

            if (await _centroeducativoService.ExisteCentroEducativo(centro.NombreCentro, centro.Direccion))
            {
                ViewData["Mensaje"] = "CentroDireccionYaExistente";
                return View("~/Views/Admin/Centro/Editar.cshtml", centro);
            }

            await _centroeducativoService.UpdateCentroeducativo(centro);

            return RedirectToAction("ListarCentro");
        }

        [HttpPost]
        public async Task<IActionResult> EliminarCentro([FromRoute] int Id)
        {
            try
            {
                Scaffold.Centroeducativo centro = await _centroeducativoService.DeleteCentroeducativo(Id);
                bool existe = await _centroeducativoService.ExisteCentroEducativo(centro.NombreCentro, centro.Direccion);

                if (existe)
                {
                    return Json(new { success = false, message = "El centro educativo no se pudo eliminar." });
                }
                else
                {
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Ocurrió un error al eliminar el centro educativo." });
            }
        }

        #endregion

        #region Gestion Profesores

        #endregion

        #region Gestion Alumnos

        #endregion

        #region Estudios

        [HttpGet]
        public async Task<IActionResult> ListarCiclos([FromRoute] int id)
        {
            ViewData["IdCentro"]=id;


            IEnumerable<CicloformativoModel> listado = await _cicloformativoService.ListadoCicloformativo(id);
            return View("~/Views/Admin/Ciclos/Index.cshtml",listado);
        }

        [HttpGet]
        public IActionResult GuardarCiclo([FromRoute] int id)
        {
            ViewData["IdCentro"]=id;


            return View("~/Views/Admin/Ciclos/Guardar.cshtml", new CicloformativoModel { IdCentro=id });

        }


        [HttpPost]
        public async Task<IActionResult> GuardarCiclo(CicloformativoModel datosCiclo)
        {
            ViewData["IdCentro"]=datosCiclo.IdCentro;
            
            if (!ModelState.IsValid) 
            { 
                return View("~/Views/Admin/Ciclo/Guardar.cshtml", datosCiclo); 
            }              
            else if (await _cicloformativoService.ExistCicloformativo(datosCiclo))
            {
                ViewData["Mensaje"] = "El nombre del ciclo o acrónimo ya existen";
                return View("~/Views/Admin/Ciclos/Guardar.cshtml", datosCiclo );
            }
            else
            {
                await _cicloformativoService.CreateCicloformativo(datosCiclo);
                return RedirectToAction("ListarCiclos", new { id = datosCiclo.IdCentro });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditarCiclo([FromRoute] int id)
        {
            ViewData["IdCentro"]=id;
            CicloformativoModel cicloformativo = await _cicloformativoService.ReadCicloformativo(id);

            return View("~/Views/Admin/Ciclos/Editar.cshtml", cicloformativo);
        }


        [HttpPost]
        public async Task<IActionResult> EditarCiclo(CicloformativoModel datosCiclo)
        {
            ViewData["IdCentro"]=datosCiclo.IdCentro;

            if (await _cicloformativoService.ExistCicloformativo(datosCiclo))
            {
                ViewData["Mensaje"] = "El nombre del ciclo o acrónimo ya existen";
                return View("~/Views/Admin/Ciclos/Editar.cshtml", datosCiclo);
            }

            await _cicloformativoService.UpdateCicloformativo(datosCiclo);

            return RedirectToAction("ListarCiclos", new { id = datosCiclo.IdCentro });

        }

        [HttpPost]
        public async Task<IActionResult> EliminarCiclo([FromRoute] int id)
        {
            try
            {
                CicloformativoModel ciclo =await _cicloformativoService.ReadCicloformativo(id);
                await _cicloformativoService.DeleteCicloformativo(ciclo);
                bool existe = await _cicloformativoService.ExistCicloformativo(ciclo);

                if (existe)
                {
                    return Json(new { success = false, message = "El ciclo educativo no se pudo eliminar." });
                }
                else
                {
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Ocurrió un error al eliminar el ciclo formativo." });
            }
        }
        [HttpGet]
        public async Task<IActionResult> InformacionCiclo([FromRoute] int id)
        {
            ViewData["IdCiclo"]=id;


            //IEnumerable<CicloformativoModel> listado = await _cicloformativoService.ListadoCicloformativo(id);
            return View("~/Views/Admin/Ciclos/Informacion.cshtml");
        }

        #endregion

        #region Estudios

        [HttpGet]
        public async Task<IActionResult> ListarAsignaturas([FromRoute] int id)
        {
            ViewData["IdAsignatura"]=id;


            //IEnumerable<CicloformativoModel> listado = await _cicloformativoService.ListadoCicloformativo(id);
            return View("~/Views/Admin/Asignaturas/Index.cshtml");
        }

        #endregion

    }
}