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
    [Authorize(Roles = "ALUMNO")]
    public class AlumnoController : Controller
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

        public AlumnoController(
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











        public IActionResult ListarAsignaturasEstudiosAlumno()
        {
            ClaimsPrincipal claimsUser = HttpContext.User;
            string emailUsuario = "";
            if (claimsUser.Identity!.IsAuthenticated)
                emailUsuario = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault()!;


            return View();
        }


        public async Task<IActionResult> MostrarHorarioAlumno()
        {
            ClaimsPrincipal claimsUser = HttpContext.User;
            string emailUsuario = "";
            if (claimsUser.Identity!.IsAuthenticated)
                emailUsuario = claimsUser.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault()!;
            Scaffold.Usuario alumno = await _usuarioService.GetUsuario(emailUsuario);

            ViewData["controller"] ="Home";
            ViewData["action"]= "Index";

            AlumnoModel alumnoModel = await _alumnoService.ReadAlumno(alumno.IdUsuario);
            ViewData["idCentro"]=alumnoModel.IdCentro;

            ViewData["DiasSemana"] = await _diasemanaService.ListDiasemana();
            ViewData["Horas"] = await _franjahorariumService.ListFranjahorarium();

            ViewData["Horarios"] = await _matriculasalumnoService.ListHorariosAlumno(alumno.IdUsuario);
            ViewData["ListaAsignaturas"] = await _matriculasalumnoService.ListAsignaturasAlumno(alumno.IdUsuario);
            ViewData["ListaAulas"] = await _matriculasalumnoService.ListAulasAlumno(alumno.IdUsuario);

            ViewData["ListaEstudios"] = await _matriculasalumnoService.ListEstudiosAlumno(alumno.IdUsuario);

            return View("~/Views/Admin/Horarios/Horario.cshtml");
        }
    }
}
