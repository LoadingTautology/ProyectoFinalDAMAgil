﻿using Microsoft.AspNetCore.Authorization;
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
            ViewData["DireccionCentro"] = centroDB.Direccion;

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

        [HttpGet]
        public async Task<IActionResult> ListarHorarios([FromRoute] int id, int idEstudios)
        {
            try
            {
                await SetViewDataForHorarios(id, idEstudios);
                var horarioAsigEstudio = await _horarioService.ListHorariosAsignaturaEstudio(id, idEstudios);

                if (!horarioAsigEstudio.Any())
                {
                    return View("~/Views/Admin/Horarios/Index.cshtml");
                }
                else
                {
                    return View("~/Views/Admin/Horarios/Index.cshtml", horarioAsigEstudio.First());
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores y logging
                ViewData["Mensaje"] = "Ocurrió un error al cargar los horarios.";
                return View("~/Views/Admin/Horarios/Index.cshtml");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GuardarHorario(HorarioModel modeloHorario, int? eliminarIdDiaFranja)
        {
            try
            {
                if (eliminarIdDiaFranja.HasValue)
                {
                    await EliminarHorario(eliminarIdDiaFranja.Value, modeloHorario);
                }
                else
                {
                    await GuardarOActualizarHorario(modeloHorario);
                }

                return await CargarDatosVista(modeloHorario);
            }
            catch (Exception ex)
            {
                // Manejo de errores y logging
                ViewData["Mensaje"] = "Ocurrió un error al guardar el horario.";
                return await CargarDatosVista(modeloHorario);
            }
        }

        private async Task EliminarHorario(int eliminarIdDiaFranja, HorarioModel modeloHorario)
        {
            var horarioAEliminar = await _horarioService.ReadHorario(eliminarIdDiaFranja, modeloHorario.IdAsignatura, modeloHorario.IdEstudio);
            if (horarioAEliminar != null)
            {
                await _horarioService.DeleteHorario(horarioAEliminar);
            }
        }

        private async Task GuardarOActualizarHorario(HorarioModel modeloHorario)
        {
            if (await _horarioService.ExistHorario(modeloHorario.IdDiaFranja, modeloHorario.IdAsignatura, modeloHorario.IdEstudio))
            {
                var hor = await _horarioService.ReadHorario(modeloHorario.IdDiaFranja, modeloHorario.IdAsignatura, modeloHorario.IdEstudio);
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
        }

        private async Task<IActionResult> CargarDatosVista(HorarioModel modeloHorario)
        {
            try
            {
                await SetViewDataForHorarios(modeloHorario.IdAsignatura, modeloHorario.IdEstudio);
                return View("~/Views/Admin/Horarios/Index.cshtml", modeloHorario);
            }
            catch (Exception ex)
            {
                // Manejo de errores y logging
                ViewData["Mensaje"] = "Ocurrió un error al cargar los datos de la vista.";
                return View("~/Views/Admin/Horarios/Index.cshtml", modeloHorario);
            }
        }

        private async Task SetViewDataForHorarios(int idAsignatura, int idEstudios)
        {
            ViewData["idAsignatura"] = idAsignatura;
            ViewData["idEstudios"] = idEstudios;
            ViewData["DiasSemana"] = await _diasemanaService.ListDiasemana();
            ViewData["Horas"] = await _franjahorariumService.ListFranjahorarium();

            var cicloformativo = await _cicloformativoService.ReadCicloformativo(idEstudios);
            ViewData["ListaAulas"] = await _aulaService.ListadoAulas(cicloformativo.IdCentro);
            ViewData["idCentro"] = cicloformativo.IdCentro;

            var asignaturaModel = await _asignaturaService.ReadAsignatura(idAsignatura);
            ViewData["NombreAsignatura"] = asignaturaModel.NombreAsignatura;
            ViewData["CursoAsignatura"] = asignaturaModel.Curso;
            ViewData["Horarios"] = await _horarioService.ListHorariosEstudioCursoAsignatura(asignaturaModel.Curso, idEstudios);
            ViewData["ListaAsignaturas"] = await _asignaturaService.ListadoAsignatura(idEstudios);
        }




        #endregion


        /* ********************PROFESORES******************** */
        #region Gestion Profesores

        [HttpGet]
        public async Task<IActionResult> ListarCentroProfesores()
        {

            ClaimsPrincipal claimsUser = HttpContext.User;
            string emailUsuario = "";
            if (claimsUser.Identity!.IsAuthenticated)
                emailUsuario = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault()!;

            IEnumerable<Scaffold.Centroeducativo> listado = await _centroeducativoService.ListadoCentroEducativo(emailUsuario);
            ViewData["Action"]="ListarProfesores";
            ViewData["TipoUsuario"]="Profesores";

            return View("~/Views/Admin/Centro/Index.cshtml", listado);
        }


        [HttpGet]
        public async Task<IActionResult> ListarProfesores(int id)
        {

            ViewData["idCentro"]=id;
            var centroDB = await _centroeducativoService.GetCentroeducativo(id);
            ViewData["NombreCentro"] = centroDB.NombreCentro;
            ViewData["DireccionCentro"] = centroDB.Direccion;


            IEnumerable<ProfesorModel> listadoProfesores = await _profesorService.ListadoProfesores(id);

            return View("~/Views/Admin/Profesores/Index.cshtml", listadoProfesores);
        }

        [HttpGet]
        public IActionResult GuardarProfesor(int id)//IdCentro
        {
            ViewData["idCentro"]=id;
            return View("~/Views/Admin/Profesores/Guardar.cshtml");
        }


        [HttpPost]
        public async Task<IActionResult> GuardarProfesor(ProfesorModel profesorModel)
        {
            //Console.WriteLine("**************Guardar Profesor: "+profesorModel.ToString() );
            ViewData["idCentro"]=profesorModel.IdCentro;

            if (!ModelState.IsValid)
            {
                return View("~/Views/Admin/Profesores/Guardar.cshtml", profesorModel);
            }
            else if (await _correoelectronicoService.ExistCorreoElectronico(profesorModel.Email))
            {
                profesorModel.Email ="";
                ViewData["Mensaje"] = "Ese email ya existe";
                return View("~/Views/Admin/Profesores/Guardar.cshtml", profesorModel);
            }
            else
            {
                try
                {
                    await _profesorService.CreateProfesor(profesorModel,"123");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            return RedirectToAction("ListarProfesores", new { id = profesorModel.IdCentro });
        }

        [HttpPost]
        public async Task<IActionResult> EliminarProfesor([FromRoute] int id)//idProfesor
        {
            try
            {
                ProfesorModel profesorModel = await _profesorService.ReadProfesor(id);

                Scaffold.Correoelectronico correoDB = await _correoelectronicoService.DeleteCorreoElectronico(profesorModel.Email);
                bool existe = await _correoelectronicoService.ExistCorreoElectronico(profesorModel.Email);

                if (existe)
                {
                    return Json(new { success = false, message = "El profesor no se pudo eliminar." });
                }
                else
                {
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Ocurrió un error al eliminar el profesor." });
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditarProfesor([FromRoute] int id)//idProfesor
        {
            ProfesorModel profesorModel = await _profesorService.ReadProfesor(id);

            return View("~/Views/Admin/Profesores/Editar.cshtml", profesorModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditarProfesor(ProfesorModel profesorModel)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Admin/Profesores/Editar.cshtml", profesorModel);
            }

            await _profesorService.UpdateProfesor(profesorModel);

            return RedirectToAction("ListarProfesores", new { id = profesorModel.IdCentro });
        }


        #endregion


        /* ********************ALUMNOS******************** */
        #region Gestion Alumnos

        [HttpGet]
        public async Task<IActionResult> ListarCentroAlumnos()
        {

            ClaimsPrincipal claimsUser = HttpContext.User;
            string emailUsuario = "";
            if (claimsUser.Identity!.IsAuthenticated)
                emailUsuario = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault()!;

            IEnumerable<Scaffold.Centroeducativo> listado = await _centroeducativoService.ListadoCentroEducativo(emailUsuario);
            ViewData["Action"]="ListarAlumnos";
            ViewData["TipoUsuario"]="Alumnos";

            return View("~/Views/Admin/Centro/Index.cshtml", listado);
        }

        [HttpGet]
        public async Task<IActionResult> ListarAlumnos(int id)
        {

            ViewData["idCentro"]=id;
            var centroDB = await _centroeducativoService.GetCentroeducativo(id);
            ViewData["NombreCentro"] = centroDB.NombreCentro;
            ViewData["DireccionCentro"] = centroDB.Direccion;

            IEnumerable<AlumnoModel> listadoAlumnos = await _alumnoService.ListadoAlumnos(id);

            return View("~/Views/Admin/Alumnos/Index.cshtml", listadoAlumnos);
        }

        [HttpGet]
        public IActionResult GuardarAlumno(int id)//IdCentro
        {
            ViewData["idCentro"]=id;
            return View("~/Views/Admin/Alumnos/Guardar.cshtml");
        }


        [HttpPost]
        public async Task<IActionResult> GuardarAlumno(AlumnoModel alumnoModel)
        {

            ViewData["idCentro"]=alumnoModel.IdCentro;

            if (!ModelState.IsValid)
            {
                return View("~/Views/Admin/Alumnos/Guardar.cshtml", alumnoModel);
            }
            else if (await _correoelectronicoService.ExistCorreoElectronico(alumnoModel.Email))
            {
                alumnoModel.Email ="";
                ViewData["Mensaje"] = "Ese email ya existe";
                return View("~/Views/Admin/Alumnos/Guardar.cshtml", alumnoModel);
            }
            else
            {
                try
                {
                    await _alumnoService.CreateAlumno(alumnoModel, "123");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            return RedirectToAction("ListarAlumnos", new { id = alumnoModel.IdCentro });
        }

        [HttpPost]
        public async Task<IActionResult> EliminarAlumno([FromRoute] int id)//idAlumno
        {
            try
            {
                AlumnoModel alumnoModel = await _alumnoService.ReadAlumno(id);

                Scaffold.Correoelectronico correoDB = await _correoelectronicoService.DeleteCorreoElectronico(alumnoModel.Email);
                bool existe = await _correoelectronicoService.ExistCorreoElectronico(alumnoModel.Email);

                if (existe)
                {
                    return Json(new { success = false, message = "El alumno no se pudo eliminar." });
                }
                else
                {
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Ocurrió un error al eliminar el alumno." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditarAlumno([FromRoute] int id)//idAlumno
        {
            AlumnoModel alumnoModel = await _alumnoService.ReadAlumno(id);

            return View("~/Views/Admin/Alumnos/Editar.cshtml", alumnoModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditarAlumno(AlumnoModel alumnoModel)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Admin/Alumnos/Editar.cshtml", alumnoModel);
            }

            await _alumnoService.UpdateAlumno(alumnoModel);

            return RedirectToAction("ListarAlumnos",new {id=alumnoModel.IdCentro });
        }


        #endregion


        /* ********************ASIGNAR_PROFESOR******************** */
        #region ASIGNAR_PROFESOR

        public async Task<IActionResult> ListarEstudiosProfesor(int idProfesor)
        {
            ProfesorModel profesorModel = await _profesorService.ReadProfesor(idProfesor);

            ViewData["NombreUsuario"]=profesorModel.NombreUsuario;
            ViewData["ApellidosUsuario"]=profesorModel.ApellidosUsuario;
            ViewData["IdProfesor"]= profesorModel.IdProfesor;
            ViewData["IdCentro"]=profesorModel.IdCentro;

            //List<AsignaturasprofesorModel> listaAsigProfModel = new List<AsignaturasprofesorModel>();
            //listaAsigProfModel.Add(new AsignaturasprofesorModel() { NombreUsuario = "Jose", ApellidosUsuario ="Navarro",  IdCentro=idCentro });

            return View("~/Views/Admin/Profesores/ListarEstudiosAsignaturas.cshtml");
        }

        public async Task<IActionResult> ListarEstudiosAsignar(int idProfesor)
        {
            ViewData["IdProfesor"]= idProfesor;
            ProfesorModel profesorModel = await _profesorService.ReadProfesor(idProfesor);
            ViewData["IdCentro"]=profesorModel.IdCentro;

            var centroDB = await _centroeducativoService.GetCentroeducativo(profesorModel.IdCentro);
            ViewData["NombreCentro"] = centroDB.NombreCentro;
            ViewData["DireccionCentro"] = centroDB.Direccion;

            IEnumerable<CicloformativoModel> listadoEstudios = await _cicloformativoService.ListadoCicloformativo(profesorModel.IdCentro);

            return View("~/Views/Admin/Profesores/ListarEstudios.cshtml",listadoEstudios);
        }

        public async Task<IActionResult> ListarAsignaturasAsignar(int idProfesor, int idEstudio)
        {
            ViewData["idProfesor"]= idProfesor;
            ViewData["idEstudio"]=idEstudio;

            CicloformativoModel cicloformativo = await _cicloformativoService.ReadCicloformativo(idEstudio);
            ViewData["Acronimo"] = cicloformativo.Acronimo;
            IEnumerable<AsignaturaModel> listadoAsignaturas = await _asignaturaService.ListadoAsignatura(idEstudio);
            /*HAY QUE MODIFICARLO ASIGNATURAS DEL ESTUDIO DIFERENCIA ASIGNADAS*/
            return View("~/Views/Admin/Profesores/ListarAsignaturas.cshtml", listadoAsignaturas);
        }

        public async Task<IActionResult> AsignarProfesorAsignatura(int idProfesor, int idEstudio, int idAsignatura)
        {            
            return View(/*"~/Views/Admin/Profesores/ListarAsignaturas.cshtml"*/);
        }

        

        #endregion

        /* ********************MATRICULAR_ALUMNOS******************** */
        #region MATRICULAR_ALUMNOS
        public async Task<IActionResult> ListarEstudiosAlumno(int idAlumno)
        {
            AlumnoModel alumnoModel = await _alumnoService.ReadAlumno(idAlumno);

            ViewData["NombreUsuario"]=alumnoModel.NombreUsuario;
            ViewData["ApellidosUsuario"]=alumnoModel.ApellidosUsuario;
            ViewData["IdAlumno"]= alumnoModel.IdAlumno;
            ViewData["IdCentro"]=alumnoModel.IdCentro;

            //List<AsignaturasprofesorModel> listaAsigProfModel = new List<AsignaturasprofesorModel>();
            //listaAsigProfModel.Add(new AsignaturasprofesorModel() { NombreUsuario = "Jose", ApellidosUsuario ="Navarro",  IdCentro=idCentro });

            return View("~/Views/Admin/Alumnos/ListarEstudiosAsignaturas.cshtml");
        }

        public async Task<IActionResult> ListarEstudiosMatricular(int idAlumno)
        {
            ViewData["IdAlumno"]= idAlumno;
            AlumnoModel alumnoModel = await _alumnoService.ReadAlumno(idAlumno);
            ViewData["IdCentro"]=alumnoModel.IdCentro;

            var centroDB = await _centroeducativoService.GetCentroeducativo(alumnoModel.IdCentro);
            ViewData["NombreCentro"] = centroDB.NombreCentro;
            ViewData["DireccionCentro"] = centroDB.Direccion;

            IEnumerable<CicloformativoModel> listadoEstudios = await _cicloformativoService.ListadoCicloformativo(alumnoModel.IdCentro);

            return View("~/Views/Admin/Alumnos/ListarEstudios.cshtml", listadoEstudios);
        }

        public async Task<IActionResult> ListarAsignaturasMatricular(int idAlumno, int idEstudio)
        {
            ViewData["idAlumno"]= idAlumno;
            ViewData["idEstudio"]= idEstudio;

            CicloformativoModel cicloformativo = await _cicloformativoService.ReadCicloformativo(idEstudio);
            ViewData["Acronimo"] = cicloformativo.Acronimo;
            IEnumerable<AsignaturaModel> listadoAsignaturas = await _asignaturaService.ListadoAsignatura(idEstudio);
            /*HAY QUE MODIFICARLO ASIGNATURAS DEL ESTUDIO DIFERENCIA MATRICULADAS*/

            return View("~/Views/Admin/Alumnos/ListarAsignaturas.cshtml", listadoAsignaturas);
        }

        public async Task<IActionResult> MatricularAlumnoAsignatura(int idAlumno, int idEstudio, int idAsignatura)
        {
            return View(/*"~/Views/Admin/Alumnos/ListarAsignaturas.cshtml"*/);
        }


        

        #endregion


    }
}