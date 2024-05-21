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
using ProyectoFinalDAMAgil.Services.Asignatura;
using System.Collections.Generic;

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
        private readonly IAsignaturaService _asignaturaService;

        public AdminController(IUsuarioService usuarioService,
                               ICorreoelectronicoService correoelectronicoService,
                               IAdministradorService administradorService,
                               ICentroeducativoService centroeducativoService,
                               IUsuarioscentroeducativoService usuarioscentroeducativo,
                               ICicloformativoService cicloformativoService,
                               IAsignaturaService asignaturaService)
        {
            _usuarioService = usuarioService;
            _correoelectronicoService = correoelectronicoService;
            _administradorService = administradorService;
            _centroeducativoService = centroeducativoService;
            _usuarioscentroeducativoService = usuarioscentroeducativo;
            _cicloformativoService = cicloformativoService;
            _asignaturaService = asignaturaService;

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
                ViewData["Mensaje"] = "CentroDireccionYaExistente";
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

        /* ********************ESTUDIOS******************** */
        #region Estudios

        [HttpGet]
        public async Task<IActionResult> ListarEstudios([FromRoute] int id)
        {

            ViewData["IdCentro"]=id;

            IEnumerable<CicloformativoModel> listado = await _cicloformativoService.ListadoCicloformativo(id);
            return View("~/Views/Admin/Estudios/Index.cshtml", listado);
        }

        [HttpGet]
        public IActionResult GuardarEstudios([FromRoute] int id)
        {
            ViewData["IdCentro"]=id;


            return View("~/Views/Admin/Estudios/Guardar.cshtml", new CicloformativoModel { IdCentro=id });

        }


        [HttpPost]
        public async Task<IActionResult> GuardarEstudios(CicloformativoModel datosCiclo)
        {
            ViewData["IdCentro"]=datosCiclo.IdCentro;

            if (!ModelState.IsValid)
            {
                return View("~/Views/Admin/Estudios/Guardar.cshtml", datosCiclo);
            }
            else if (await _cicloformativoService.ExistCicloformativo(datosCiclo))
            {
                ViewData["Mensaje"] = "El nombre del ciclo o acrónimo ya existen";
                return View("~/Views/Admin/Estudios/Guardar.cshtml", datosCiclo);
            }
            else
            {
                await _cicloformativoService.CreateCicloformativo(datosCiclo);
                return RedirectToAction("ListarEstudios", new { id = datosCiclo.IdCentro });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditarEstudios([FromRoute] int id)
        {
            ViewData["IdCentro"]=id;
            CicloformativoModel cicloformativo = await _cicloformativoService.ReadCicloformativo(id);

            return View("~/Views/Admin/Estudios/Editar.cshtml", cicloformativo);
        }


        [HttpPost]
        public async Task<IActionResult> EditarEstudios(CicloformativoModel datosCiclo)
        {
            ViewData["IdCentro"]=datosCiclo.IdCentro;

            if (await _cicloformativoService.ExistCicloformativo(datosCiclo))
            {
                ViewData["Mensaje"] = "El nombre del ciclo o acrónimo ya existen";
                return View("~/Views/Admin/Estudios/Editar.cshtml", datosCiclo);
            }

            await _cicloformativoService.UpdateCicloformativo(datosCiclo);

            return RedirectToAction("ListarEstudios", new { id = datosCiclo.IdCentro });

        }

        [HttpPost]
        public async Task<IActionResult> EliminarEstudios([FromRoute] int id)
        {
            try
            {
                CicloformativoModel ciclo = await _cicloformativoService.ReadCicloformativo(id);
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






        #endregion

        /* ********************ASIGNATURAS******************** */
        #region Asignaturas

        [HttpGet]
        public async Task<IActionResult> ListarAsignaturas([FromRoute] int id)
        {

            ViewData["idEstudios"]=id;

            CicloformativoModel cicloformativo = await _cicloformativoService.ReadCicloformativo(id);
            ViewData["IdCentro"]= cicloformativo.IdCentro;

            IEnumerable<AsignaturaModel> listadoAsignaturas = await _asignaturaService.ListadoAsignatura(id);

            return View("~/Views/Admin/Asignaturas/Index.cshtml", listadoAsignaturas);
        }

        [HttpGet]
        public IActionResult GuardarAsignatura([FromRoute] int id)
        {
            ViewData["idCiclo"]=id;

            return View("~/Views/Admin/Asignaturas/Guardar.cshtml");

        }


        [HttpPost]
        public async Task<IActionResult> GuardarAsignatura([FromRoute] int id, AsignaturaModel datosAsignaturas)
        {
            ViewData["idCiclo"]=id;

            CicloformativoModel cicloformativo = await _cicloformativoService.ReadCicloformativo(id);

            if (!ModelState.IsValid)
            {
                return View("~/Views/Admin/Asignaturas/Guardar.cshtml", datosAsignaturas);
            }
            else if (await _asignaturaService.ExistAsignatura(datosAsignaturas, id, cicloformativo.IdCentro))
            {
                ViewData["Mensaje"] = "El nombre de la asignatura ya existe en este centro";
                return View("~/Views/Admin/Asignaturas/Guardar.cshtml", datosAsignaturas);
            }
            else
            {
                await _asignaturaService.CreateAsignatura(datosAsignaturas,id);
                return RedirectToAction("ListarAsignaturas", new { id = id} );
            }
        }



        [HttpGet]
        public async Task<IActionResult> EditarAsignatura([FromRoute] int id)
        {
            int idAsignatura = id;           
            AsignaturaModel asignaturaModel = await _asignaturaService.ReadAsignatura(idAsignatura);
            IEnumerable<CicloformativoModel> listadoCiclosIEnumerable = await _asignaturaService.ListadoCiclos(asignaturaModel);
            CicloformativoModel cicloformativoModel= listadoCiclosIEnumerable.ToList().FirstOrDefault();
            ViewData["idCiclo"]= cicloformativoModel.IdCiclo;

            return View("~/Views/Admin/Asignaturas/Editar.cshtml", asignaturaModel);
        }


        [HttpPost]
        public async Task<IActionResult> EditarAsignatura([FromRoute] int id, AsignaturaModel datosAsignatura)
        {
            ViewData["idCiclo"]=id;            
            CicloformativoModel cicloformativo = await _cicloformativoService.ReadCicloformativo(id);

            if (await _asignaturaService.ExistAsignatura(datosAsignatura,id , cicloformativo.IdCentro))
            {
                ViewData["Mensaje"] = "El nombre de la asignatura ya existe en este centro";
                return View("~/Views/Admin/Asignaturas/Guardar.cshtml", datosAsignatura);
            }

            await _asignaturaService.UpdateAsignatura(datosAsignatura);

            return RedirectToAction("ListarAsignaturas", new { id = id });

        }

        [HttpPost]
        public async Task<IActionResult> EliminarAsignatura([FromRoute] int id, int idEstudios)
        {
            int idAsignatura = id;
            try
            {
                AsignaturaModel asignaturaModel = await _asignaturaService.ReadAsignatura(idAsignatura);
                CicloformativoModel cicloformativo = await _cicloformativoService.ReadCicloformativo(idEstudios);

                await _asignaturaService.DeleteAsignatura(asignaturaModel,idEstudios);
                bool existe = await _asignaturaService.ExistAsignatura(asignaturaModel, idEstudios, cicloformativo.IdCentro) ;

                if (existe)
                {
                    return Json(new { success = false, message = "La asignatura no se pudo eliminar." });
                }
                else
                {
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Ocurrió un error al eliminar la asignatura." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListarAsignaturasCentro([FromRoute] int id)
        {
            ViewData["idEstudios"]=id;
            CicloformativoModel cicloformativo = await _cicloformativoService.ReadCicloformativo(id);
            ViewData["idCentros"]= cicloformativo.IdCentro;

            IEnumerable<AsignaturaModel> listadoAsignaturasCicloIEnumerable = await _asignaturaService.ListadoAsignatura(id);
            List<int> idAsignaturasCiclo = new List<int>();
            foreach (var item in listadoAsignaturasCicloIEnumerable)
            {
                idAsignaturasCiclo.Add(item.IdAsignatura);
            }

            IEnumerable<AsignaturaModel> listadoAsignaturasCentroIEnumerable = await _asignaturaService.ListadoAsignaturaCentro(cicloformativo.IdCentro);
            List<int> idAsignaturasCentro = new List<int>();
            foreach (var item in listadoAsignaturasCentroIEnumerable)
            {
                idAsignaturasCentro.Add(item.IdAsignatura);
            }

            IEnumerable<int> idlistadoCentroDiferenciaCiclo = from idLista in idAsignaturasCentro.Except(idAsignaturasCiclo) select idLista;
            List<AsignaturaModel> listadoCentroDiferenciaCiclo = new List<AsignaturaModel>();
            foreach (int item in idlistadoCentroDiferenciaCiclo)
            {
                listadoCentroDiferenciaCiclo.Add(await _asignaturaService.ReadAsignatura(item));
            }

            return View("~/Views/Admin/Asignaturas/AddExistente.cshtml", listadoCentroDiferenciaCiclo);
        }

        public async Task<IActionResult> GuardarAsignaturaExistente([FromRoute] int id, int idEstudios)//, IEnumerable<AsignaturaModel> datosAsignaturas)
        {

            AsignaturaModel asignaturaModel = await _asignaturaService.ReadAsignatura(id);
            await _asignaturaService.VincularAsignaturaCiclo(asignaturaModel, idEstudios);

            return RedirectToAction("ListarAsignaturas", new { id = idEstudios }); //idCiclo
        }


        public async Task<IActionResult> InformacionAsignatura() 
        {
            return View("~/Views/Admin/Asignaturas/Informacion.cshtml");
        }

        #endregion

        #region Gestion Profesores

        #endregion

        #region Gestion Alumnos

        #endregion

    }
}