using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinalDAMAgil.Models.Admin;
using ProyectoFinalDAMAgil.Scaffold;
using ProyectoFinalDAMAgil.Services.Administrador;
using ProyectoFinalDAMAgil.Services.Alumno;
using ProyectoFinalDAMAgil.Services.Asignatura;
using ProyectoFinalDAMAgil.Services.AsignaturasEstudios;
using ProyectoFinalDAMAgil.Services.Asignaturasprofesor;
using ProyectoFinalDAMAgil.Services.Aula;
using ProyectoFinalDAMAgil.Services.Centroeducativo;
using ProyectoFinalDAMAgil.Services.Cicloformativo;
using ProyectoFinalDAMAgil.Services.Correoelectronico;
using ProyectoFinalDAMAgil.Services.Diasemana;
using ProyectoFinalDAMAgil.Services.Franjahorarium;
using ProyectoFinalDAMAgil.Services.Horario;
using ProyectoFinalDAMAgil.Services.Matriculasalumno;
using ProyectoFinalDAMAgil.Services.Profesor;
using ProyectoFinalDAMAgil.Services.Usuario;
using ProyectoFinalDAMAgil.Services.Usuarioscentroeducativo;
using System.Security.Claims;

namespace ProyectoFinalDAMAgil.Controllers
{
    [Authorize(Roles = "PROFESOR")]
    public class ProfesorController : Controller
    {

        private readonly IAdministradorService _administradorService;
        private readonly IAlumnoService _alumnoService;
        private readonly IAsignaturaService _asignaturaService;
        private readonly IAsignaturasEstudiosService _asignaturasEstudiosService;
        private readonly IAsignaturasprofesorService _asignaturaprofesorService;
        private readonly IAulaService _aulaService;
        private readonly ICentroeducativoService _centroeducativoService;
        private readonly ICicloformativoService _cicloformativoService;
        private readonly ICorreoelectronicoService _correoelectronicoService;
        private readonly IDiasemanaService _diasemanaService;
        private readonly IFranjahorariumService _franjahorariumService;
        private readonly IHorarioService _horarioService;
        private readonly IMatriculasalumnoService _matriculasalumnoService;
        private readonly IProfesorService _profesorService;
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioscentroeducativoService _usuarioscentroeducativoService;

        public ProfesorController(
                                IAdministradorService administradorService,
                                IAlumnoService alumnoService,
                                IAsignaturaService asignaturaService,
                                IAsignaturasEstudiosService asignaturasEstudiosService,
                                IAsignaturasprofesorService asignaturasprofesorService,
                                IAulaService aulaService,
                                ICentroeducativoService centroeducativoService,
                                ICicloformativoService cicloformativoService,
                                ICorreoelectronicoService correoelectronicoService,
                                IDiasemanaService diasemanaService,
                                IFranjahorariumService franjahorariumService,
                                IHorarioService horarioService,
                                IMatriculasalumnoService matriculasalumnoService,
                                IProfesorService profesorService,
                                IUsuarioService usuarioService,
                                IUsuarioscentroeducativoService usuarioscentroeducativo)


        {
            _administradorService = administradorService;
            _alumnoService = alumnoService;
            _asignaturaService = asignaturaService;
            _asignaturasEstudiosService = asignaturasEstudiosService;
            _asignaturaprofesorService = asignaturasprofesorService;
            _aulaService = aulaService;
            _centroeducativoService = centroeducativoService;
            _cicloformativoService = cicloformativoService;
            _correoelectronicoService = correoelectronicoService;
            _diasemanaService = diasemanaService;
            _franjahorariumService = franjahorariumService;
            _horarioService = horarioService;
            _matriculasalumnoService = matriculasalumnoService;
            _profesorService = profesorService;
            _usuarioService = usuarioService;
            _usuarioscentroeducativoService = usuarioscentroeducativo;
        }











        public async Task<IActionResult> ListarAsignaturasEstudiosProfesor()
        {
            ClaimsPrincipal claimsUser = HttpContext.User;
            string emailUsuario = "";
            if (claimsUser.Identity!.IsAuthenticated)
                emailUsuario = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault()!;
            Scaffold.Usuario usuario = await _usuarioService.GetUsuario(emailUsuario);

            ProfesorModel profesorModel = await _profesorService.ReadProfesor(usuario.IdUsuario);

            ViewData["NombreUsuario"]=profesorModel.NombreUsuario;
            ViewData["ApellidosUsuario"]=profesorModel.ApellidosUsuario;
            ViewData["IdProfesor"]= profesorModel.IdProfesor;
            IEnumerable<AsignaturasprofesorModel> asignaturasprofesorModels = await _asignaturaprofesorService.ListAsignaturasAsignadasProfesor(usuario.IdUsuario);

            return View("~/Views/Profesor/Asignaturas.cshtml", asignaturasprofesorModels);
        }


        public async Task<IActionResult> MostrarHorarioProfesor()
        {
            ClaimsPrincipal claimsUser = HttpContext.User;
            string emailUsuario = "";
            if (claimsUser.Identity!.IsAuthenticated)
                emailUsuario = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault()!;
            Scaffold.Usuario profesor = await _usuarioService.GetUsuario(emailUsuario);

            ViewData["controller"] ="Home";
            ViewData["action"]= "Index";
            ProfesorModel profesorModel = await _profesorService.ReadProfesor(profesor.IdUsuario);
            ViewData["idCentro"]=profesorModel.IdCentro;

            ViewData["DiasSemana"] = await _diasemanaService.ListDiasemana();
            ViewData["Horas"] = await _franjahorariumService.ListFranjahorarium();

            ViewData["Horarios"] = await _asignaturaprofesorService.ListHorariosProfesor(profesor.IdUsuario);
            ViewData["ListaAsignaturas"] = await _asignaturaprofesorService.ListAsignaturasProfesor(profesor.IdUsuario);
            ViewData["ListaAulas"] = await _asignaturaprofesorService.ListAulasProfesor(profesor.IdUsuario);

            ViewData["ListaEstudios"] = await _asignaturaprofesorService.ListEstudiosProfesor(profesor.IdUsuario);

            return View("~/Views/Admin/Horarios/Horario.cshtml");
        }



        public async Task<IActionResult> GestionarAsignatura(int idProfesor, int idEstudio, int idAsignatura)
        {
            ViewData["idProfesor"]= idProfesor;
            ViewData["idEstudio"]= idEstudio;
            ViewData["idAsignatura"]= idAsignatura;

            AsignaturasprofesorModel asignaturasprofesorModel = await _asignaturaprofesorService.ReadAsignaturasprofesor(idProfesor,idEstudio,idAsignatura);
            ViewData["acronimoEstudio"] =asignaturasprofesorModel.Acronimo;
            ViewData["nombreAsignatura"]= asignaturasprofesorModel.NombreAsignatura;

            return View("~/Views/Profesor/Asignaturas.cshtml");
        }


    }
}
