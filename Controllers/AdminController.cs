using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDAMAgil.Models.Admin;
using ProyectoFinalDAMAgil.Scaffold;
using ProyectoFinalDAMAgil.Services.Administrador;
using ProyectoFinalDAMAgil.Services.Alumno;
using ProyectoFinalDAMAgil.Services.Asignatura;
using ProyectoFinalDAMAgil.Services.AsignaturasEstudios;
using ProyectoFinalDAMAgil.Services.Aula;
using ProyectoFinalDAMAgil.Services.Centroeducativo;
using ProyectoFinalDAMAgil.Services.Cicloformativo;
using ProyectoFinalDAMAgil.Services.Correoelectronico;
using ProyectoFinalDAMAgil.Services.Diasemana;
using ProyectoFinalDAMAgil.Services.Franjahorarium;
using ProyectoFinalDAMAgil.Services.Horario;
using ProyectoFinalDAMAgil.Services.Profesor;
using ProyectoFinalDAMAgil.Services.Usuario;
using ProyectoFinalDAMAgil.Services.Usuarioscentroeducativo;
using System;
using System.Security.Claims;

namespace ProyectoFinalDAMAgil.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR")]
    public class AdminController : Controller
    {

        private readonly IAdministradorService _administradorService;
        private readonly IAlumnoService _alumnoService;
        private readonly IAsignaturaService _asignaturaService;
        private readonly IAsignaturasEstudiosService _asignaturasEstudiosService;
        private readonly IAulaService _aulaService;
        private readonly ICentroeducativoService _centroeducativoService;
        private readonly ICicloformativoService _cicloformativoService;
        private readonly ICorreoelectronicoService _correoelectronicoService;
        private readonly IDiasemanaService _diasemanaService;
        private readonly IFranjahorariumService _franjahorariumService;
        private readonly IHorarioService _horarioService;
        private readonly IProfesorService _profesorService;
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioscentroeducativoService _usuarioscentroeducativoService;

        public AdminController( 
                                IAdministradorService administradorService,
                                IAlumnoService alumnoService,
                                IAsignaturaService asignaturaService,
                                IAsignaturasEstudiosService asignaturasEstudiosService,
                                IAulaService aulaService,
                                ICentroeducativoService centroeducativoService,
                                ICicloformativoService cicloformativoService,
                                ICorreoelectronicoService correoelectronicoService,
                                IDiasemanaService diasemanaService,
                                IFranjahorariumService franjahorariumService,
                                IHorarioService horarioService,
                                IProfesorService profesorService,
                                IUsuarioService usuarioService,
                                IUsuarioscentroeducativoService usuarioscentroeducativo)


        {
            _administradorService = administradorService;
            _alumnoService = alumnoService;
            _asignaturaService = asignaturaService;
            _asignaturasEstudiosService = asignaturasEstudiosService;
            _aulaService = aulaService;
            _centroeducativoService = centroeducativoService;
            _cicloformativoService = cicloformativoService;
            _correoelectronicoService = correoelectronicoService;
            _diasemanaService = diasemanaService;
            _franjahorariumService = franjahorariumService;
            _horarioService = horarioService;
            _profesorService = profesorService;
            _usuarioService = usuarioService;
            _usuarioscentroeducativoService = usuarioscentroeducativo;
        }


        /* ********************CENTROS******************** */
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
        public async Task<IActionResult> ListarEstudios([FromRoute] int id)//idCentro
        {

            ViewData["IdCentro"]=id;

            var centroDB = await _centroeducativoService.GetCentroeducativo(id);
            ViewData["NombreCentro"] = centroDB.NombreCentro;

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
                ViewData["Mensaje"] = "EstudioYaExiste";
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
                ViewData["Mensaje"] = "EstudioYaExiste";
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
        public async Task<IActionResult> ListarAsignaturas([FromRoute] int id)//idEstudios
        {

            ViewData["idEstudios"]=id;

            CicloformativoModel cicloformativo = await _cicloformativoService.ReadCicloformativo(id);
            ViewData["IdCentro"]= cicloformativo.IdCentro;
            ViewData["AcronimoEstudio"] = cicloformativo.Acronimo;
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
                await _asignaturaService.CreateAsignatura(datosAsignaturas, id);
                return RedirectToAction("ListarAsignaturas", new { id = id });
            }
        }



        [HttpGet]
        public async Task<IActionResult> EditarAsignatura([FromRoute] int id)
        {
            int idAsignatura = id;
            AsignaturaModel asignaturaModel = await _asignaturaService.ReadAsignatura(idAsignatura);
            IEnumerable<CicloformativoModel> listadoCiclosIEnumerable = await _asignaturaService.ListadoCiclos(asignaturaModel);
            CicloformativoModel cicloformativoModel = listadoCiclosIEnumerable.ToList().FirstOrDefault();
            ViewData["idCiclo"]= cicloformativoModel.IdCiclo;

            return View("~/Views/Admin/Asignaturas/Editar.cshtml", asignaturaModel);
        }


        [HttpPost]
        public async Task<IActionResult> EditarAsignatura([FromRoute] int id, AsignaturaModel datosAsignatura)
        {
            ViewData["idCiclo"]=id;
            CicloformativoModel cicloformativo = await _cicloformativoService.ReadCicloformativo(id);

            if (await _asignaturaService.ExistAsignatura(datosAsignatura, id, cicloformativo.IdCentro))
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

                await _asignaturaService.DeleteAsignatura(asignaturaModel, idEstudios);
                bool existe = await _asignaturaService.ExistAsignatura(asignaturaModel, idEstudios, cicloformativo.IdCentro);

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

        /* ********************AULAS******************** */
        #region Aulas

        [HttpGet]
        public async Task<IActionResult> ListarAulas([FromRoute] int id)
        {
            ViewData["idCentro"] =id;

            var centroDB = await _centroeducativoService.GetCentroeducativo(id);
            ViewData["NombreCentro"] = centroDB.NombreCentro;

            IEnumerable<AulaModel> listadoAulas = await _aulaService.ListadoAulas(id);

            return View("~/Views/Admin/Aulas/Index.cshtml", listadoAulas);
        }

        [HttpGet]
        public IActionResult GuardarAula([FromRoute] int id)
        {
            ViewData["idCentro"]=id;

            return View("~/Views/Admin/Aulas/Guardar.cshtml");

        }

        [HttpPost]
        public async Task<IActionResult> GuardarAula([FromRoute] int id, AulaModel datosAulas)
        {
            datosAulas.IdCentro = id;
            
            ViewData["idCentro"]=id;

            AulaModel aulaModel = await _aulaService.ReadAula(datosAulas.NumeroAula, datosAulas.IdCentro);

            if (!ModelState.IsValid)
            {
                return View("~/Views/Admin/Aulas/Guardar.cshtml", datosAulas);
            }
            else if (await _aulaService.ExistAula(datosAulas))
            {
                ViewData["Mensaje"] = "AulaYaExiste";
                return View("~/Views/Admin/Aulas/Guardar.cshtml", datosAulas);
            }
            else
            {
                await _aulaService.CreateAula(datosAulas);
                return RedirectToAction("ListarAulas", new { id = id });
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditarAula([FromRoute] int id)//idAula
        {
            ViewData["idAula"] = id;

            AulaModel aulaModel = await _aulaService.ReadAula(id);
            ViewData["idCentro"]= aulaModel.IdCentro;

            return View("~/Views/Admin/Aulas/Editar.cshtml", aulaModel);
        }


        [HttpPost]
        public async Task<IActionResult> EditarAula([FromRoute] int id, int idAula, AulaModel aulaModel)
        {
            ViewData["idCentro"]=id;
            ViewData["idAula"] = idAula;

            if (!ModelState.IsValid)
            {
                return View("~/Views/Admin/Aulas/Editar.cshtml", aulaModel);
            }
            AulaModel aulaModelInicial = await _aulaService.ReadAula(idAula);

            if (aulaModelInicial.NumeroAula != aulaModel.NumeroAula &&  await _aulaService.ExistAula(aulaModel))
            {
                ViewData["Mensaje"] = "Ese número de aula ya existe";
                return View("~/Views/Admin/Aulas/Editar.cshtml", aulaModel);
            }

            await _aulaService.UpdateAula(aulaModel);

            return RedirectToAction("ListarAulas", new { id = id });

        }

        [HttpPost]
        public async Task<IActionResult> EliminarAula([FromRoute] int id)//idAula
        {
            try
            {
                AulaModel aulaModel = await _aulaService.ReadAula(id);
                await _aulaService.DeleteAula(aulaModel);
                bool existe = await _aulaService.ExistAula(aulaModel);

                if (existe)
                {
                    return Json(new { success = false, message = "El aula no se pudo eliminar." });
                }
                else
                {
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Json(new { success = false, message = "Ocurrió un error al eliminar el aula." });
            }
        }





        #endregion

        /* ********************HORARIOS******************** */
        #region Vincular Horarios, Asignaturas y Aulas

        [HttpGet]                                                     //idAsignatura
        public async Task<IActionResult> ListarHorarios([FromRoute] int id, int idEstudios)
        {

            ViewData["idAsignatura"]=id;
            ViewData["idEstudios"]=idEstudios;
            ViewData["DiasSemana"] = await _diasemanaService.ListDiasemana(); 

            ViewData["Horas"] = await _franjahorariumService.ListFranjahorarium();

            CicloformativoModel cicloformativo = await _cicloformativoService.ReadCicloformativo(idEstudios);
            IEnumerable<AulaModel> aulaModelList = await _aulaService.ListadoAulas(cicloformativo.IdCentro);
            ViewData["ListaAulas"] =aulaModelList ;

            AsignaturaModel asignaturaModel = await _asignaturaService.ReadAsignatura(id);
            IEnumerable<HorarioModel> horario = await _horarioService.ListHorariosEstudioCursoAsignatura(asignaturaModel.Curso, idEstudios);
            //IEnumerable<HorarioModel> horario = await _horarioService.ListHorariosEstudio(idEstudios);
            ViewData["NombreAsignatura"] = asignaturaModel.NombreAsignatura;
            ViewData["CursoAsignatura"] = asignaturaModel.Curso;

            ViewData["Horarios"] = horario;
            ViewData["ListaAsignaturas"] = await _asignaturaService.ListadoAsignatura(idEstudios);

            IEnumerable<HorarioModel> horarioAsigEstudio = await _horarioService.ListHorariosAsignaturaEstudio(id,idEstudios);

            if (horarioAsigEstudio.FirstOrDefault() ==null)
            {
                return View("~/Views/Admin/Horarios/Index.cshtml");
            }
            else 
            {
                return View("~/Views/Admin/Horarios/Index.cshtml", horarioAsigEstudio.FirstOrDefault());
            }
        }

        [HttpPost]
        public async Task<IActionResult> GuardarHorario(HorarioModel modeloHorario)
        {

            if (await _horarioService.ExistHorario(modeloHorario.IdDiaFranja, modeloHorario.IdAsignatura, modeloHorario.IdEstudio))
            {
                HorarioModel hor = await _horarioService.ReadHorario(modeloHorario.IdDiaFranja, modeloHorario.IdAsignatura, modeloHorario.IdEstudio);
                await _horarioService.DeleteHorario(hor);
            }
            else if (await _horarioService.ExistHorario(modeloHorario.IdAula, modeloHorario.IdDiaFranja))
            {
                ViewData["Mensaje"] = "El aula ya está ocupada en esa hora";
            }
            else 
            {
                await _horarioService.CambiarColor(modeloHorario);
                await _horarioService.CreateHorario(modeloHorario);
            }

            ViewData["idAsignatura"]=modeloHorario.IdAsignatura;
            ViewData["idEstudios"]=modeloHorario.IdEstudio;
            ViewData["DiasSemana"] = await _diasemanaService.ListDiasemana();

            ViewData["Horas"] = await _franjahorariumService.ListFranjahorarium();

            CicloformativoModel cicloformativo = await _cicloformativoService.ReadCicloformativo(modeloHorario.IdEstudio);
            IEnumerable<AulaModel> aulaModelList = await _aulaService.ListadoAulas(cicloformativo.IdCentro);
            ViewData["ListaAulas"] =aulaModelList;


            AsignaturaModel asignaturaModel = await _asignaturaService.ReadAsignatura(modeloHorario.IdAsignatura);
            IEnumerable<HorarioModel> horario = await _horarioService.ListHorariosEstudioCursoAsignatura(asignaturaModel.Curso, modeloHorario.IdEstudio);
            //IEnumerable<HorarioModel> horario = await _horarioService.ListHorariosEstudio(modeloHorario.IdEstudio);
            ViewData["NombreAsignatura"] = asignaturaModel.NombreAsignatura;
            ViewData["CursoAsignatura"] = asignaturaModel.Curso;
            ViewData["Horarios"] = horario;
            ViewData["ListaAsignaturas"] = await _asignaturaService.ListadoAsignatura(modeloHorario.IdEstudio);

            return View("~/Views/Admin/Horarios/Index.cshtml", modeloHorario);
        }



        #endregion


        #region Gestion Profesores

        //[HttpGet]
        //public async Task<IActionResult> ListarProfesores()
        //{

        //    ClaimsPrincipal claimsUser = HttpContext.User;
        //    string emailUsuario = "";
        //    if (claimsUser.Identity!.IsAuthenticated)
        //        emailUsuario = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault()!;

        //    /*            */
        //    IEnumerable<Scaffold.Centroeducativo> listado = await _centroeducativoService.ListadoCentroEducativo(emailUsuario);

        //    return View("~/Views/Admin/Profesores/Index.cshtml", listado);
        //}


        /* ****************************************************************************************************************************************************************************************** */
        //[HttpGet]
        //public IActionResult GuardarProfesor()
        //{
        //    return View("~/Views/Admin/Centro/Guardar.cshtml");
        //}



        //[HttpPost]
        //public async Task<IActionResult> GuardarProfesor(CentroEducativoModel datosCentro)
        //{

        //    if (!ModelState.IsValid)
        //        return View("~/Views/Admin/Centro/Guardar.cshtml", datosCentro);

        //    if (await _centroeducativoService.ExisteCentroEducativo(datosCentro.NombreCentro, datosCentro.DireccionCentro))
        //    {
        //        datosCentro.NombreCentro = "";
        //        datosCentro.DireccionCentro = "";
        //        ViewData["Mensaje"] = "CentroDireccionYaExistente";
        //        return View("~/Views/Admin/Centro/Guardar.cshtml", datosCentro);
        //    }
        //    else
        //    {
        //        try
        //        {

        //            Scaffold.Centroeducativo centroCreado = await _centroeducativoService.SaveCentroeducativo(
        //                new Scaffold.Centroeducativo
        //                {
        //                    NombreCentro = datosCentro.NombreCentro,
        //                    Direccion = datosCentro.DireccionCentro

        //                }, HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault()!
        //            );

        //            await _usuarioscentroeducativoService.SaveUsuariosCentroeducativo(
        //                new Scaffold.Usuarioscentroeducativo
        //                {
        //                    IdCentro = centroCreado.IdCentro,
        //                    IdUsuario = centroCreado.IdAdministrador
        //                }
        //            );


        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.ToString());
        //        }
        //    }

        //    return RedirectToAction("ListarCentro", "Admin");
        //}




        //[HttpGet]
        //public async Task<IActionResult> EditarCentro([FromRoute] int Id)
        //{

        //    Scaffold.Centroeducativo centro = await _centroeducativoService.GetCentroeducativo(Id);

        //    return View("~/Views/Admin/Centro/Editar.cshtml", centro);
        //}


        //[HttpPost]
        //public async Task<IActionResult> EditarCentro(Scaffold.Centroeducativo centro)
        //{

        //    if (await _centroeducativoService.ExisteCentroEducativo(centro.NombreCentro, centro.Direccion))
        //    {
        //        ViewData["Mensaje"] = "CentroDireccionYaExistente";
        //        return View("~/Views/Admin/Centro/Editar.cshtml", centro);
        //    }

        //    await _centroeducativoService.UpdateCentroeducativo(centro);

        //    return RedirectToAction("ListarCentro");
        //}

        //[HttpPost]
        //public async Task<IActionResult> EliminarCentro([FromRoute] int Id)
        //{
        //    try
        //    {
        //        Scaffold.Centroeducativo centro = await _centroeducativoService.DeleteCentroeducativo(Id);
        //        bool existe = await _centroeducativoService.ExisteCentroEducativo(centro.NombreCentro, centro.Direccion);

        //        if (existe)
        //        {
        //            return Json(new { success = false, message = "El centro educativo no se pudo eliminar." });
        //        }
        //        else
        //        {
        //            return Json(new { success = true });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = "Ocurrió un error al eliminar el centro educativo." });
        //    }
        //}
        /* ****************************************************************************************************************************************************************************************** */



        #endregion

        #region Gestion Alumnos

        //[HttpGet]
        //public async Task<IActionResult> ListarAlumnos()
        //{

        //    ClaimsPrincipal claimsUser = HttpContext.User;
        //    string emailUsuario = "";
        //    if (claimsUser.Identity!.IsAuthenticated)
        //        emailUsuario = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault()!;

        //    /*            */
        //    IEnumerable<Scaffold.Centroeducativo> listado = await _centroeducativoService.ListadoCentroEducativo(emailUsuario);

        //    return View("~/Views/Admin/Alumnos/Index.cshtml", listado);
        //}


        /* ****************************************************************************************************************************************************************************************** */
        //[HttpGet]
        //public IActionResult GuardarAlumno()
        //{
        //    return View("~/Views/Admin/Centro/Guardar.cshtml");
        //}



        //[HttpPost]
        //public async Task<IActionResult> GuardarAlumno(CentroEducativoModel datosCentro)
        //{

        //    if (!ModelState.IsValid)
        //        return View("~/Views/Admin/Centro/Guardar.cshtml", datosCentro);

        //    if (await _centroeducativoService.ExisteCentroEducativo(datosCentro.NombreCentro, datosCentro.DireccionCentro))
        //    {
        //        datosCentro.NombreCentro = "";
        //        datosCentro.DireccionCentro = "";
        //        ViewData["Mensaje"] = "CentroDireccionYaExistente";
        //        return View("~/Views/Admin/Centro/Guardar.cshtml", datosCentro);
        //    }
        //    else
        //    {
        //        try
        //        {

        //            Scaffold.Centroeducativo centroCreado = await _centroeducativoService.SaveCentroeducativo(
        //                new Scaffold.Centroeducativo
        //                {
        //                    NombreCentro = datosCentro.NombreCentro,
        //                    Direccion = datosCentro.DireccionCentro

        //                }, HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault()!
        //            );

        //            await _usuarioscentroeducativoService.SaveUsuariosCentroeducativo(
        //                new Scaffold.Usuarioscentroeducativo
        //                {
        //                    IdCentro = centroCreado.IdCentro,
        //                    IdUsuario = centroCreado.IdAdministrador
        //                }
        //            );


        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.ToString());
        //        }
        //    }

        //    return RedirectToAction("ListarCentro", "Admin");
        //}




        //[HttpGet]
        //public async Task<IActionResult> EditarCentro([FromRoute] int Id)
        //{

        //    Scaffold.Centroeducativo centro = await _centroeducativoService.GetCentroeducativo(Id);

        //    return View("~/Views/Admin/Centro/Editar.cshtml", centro);
        //}


        //[HttpPost]
        //public async Task<IActionResult> EditarCentro(Scaffold.Centroeducativo centro)
        //{

        //    if (await _centroeducativoService.ExisteCentroEducativo(centro.NombreCentro, centro.Direccion))
        //    {
        //        ViewData["Mensaje"] = "CentroDireccionYaExistente";
        //        return View("~/Views/Admin/Centro/Editar.cshtml", centro);
        //    }

        //    await _centroeducativoService.UpdateCentroeducativo(centro);

        //    return RedirectToAction("ListarCentro");
        //}

        //[HttpPost]
        //public async Task<IActionResult> EliminarCentro([FromRoute] int Id)
        //{
        //    try
        //    {
        //        Scaffold.Centroeducativo centro = await _centroeducativoService.DeleteCentroeducativo(Id);
        //        bool existe = await _centroeducativoService.ExisteCentroEducativo(centro.NombreCentro, centro.Direccion);

        //        if (existe)
        //        {
        //            return Json(new { success = false, message = "El centro educativo no se pudo eliminar." });
        //        }
        //        else
        //        {
        //            return Json(new { success = true });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = "Ocurrió un error al eliminar el centro educativo." });
        //    }
        //}
        /* ****************************************************************************************************************************************************************************************** */


        #endregion

    }
}