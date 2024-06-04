using ProyectoFinalDAMAgil.Models.Admin;
using ProyectoFinalDAMAgil.Scaffold;
using System.Security.Cryptography.Xml;

namespace ProyectoFinalDAMAgil.Services.Asignaturasprofesor
{
    public class AsignaturasprofesorService : IAsignaturasprofesorService
    {
        private readonly DbappProyectoFinalContext _context;

        public AsignaturasprofesorService(DbappProyectoFinalContext context)
        {
            _context=context;
        }

        public async Task<AsignaturasprofesorModel> CreateAsignaturasprofesor(int idProfesor, int idEstudio, int idAsignatura)
        {
            Scaffold.Asignaturascicloformativo asignaturascicloformativoDB = (from asigCiclo in _context.Asignaturascicloformativos
                                                                           where asigCiclo.IdCiclo == idEstudio && asigCiclo.IdAsignatura == idAsignatura
                                                                           select asigCiclo).FirstOrDefault()!;

            Scaffold.Asignaturasprofesor asignaturasprofesor = new Scaffold.Asignaturasprofesor()
            {
                IdProfesor=idProfesor,
                IdAsignaturasCicloFormativo = asignaturascicloformativoDB.IdAsignaturasCicloFormativo
            };
            _context.Asignaturasprofesors.Add(asignaturasprofesor);
            _context.SaveChanges();

            return new AsignaturasprofesorModel{ IdAsignatura= idAsignatura, IdCiclo= idEstudio, IdProfesor=idProfesor };
        }

        public async Task<AsignaturasprofesorModel> ReadAsignaturasprofesor(int idProfesor, int idEstudio, int idAsignatura)
        {
            IQueryable<AsignaturasprofesorModel> asignaturasprofesorListaDB =
                from asigProf in _context.Asignaturasprofesors
                join asigCiclo in _context.Asignaturascicloformativos on asigProf.IdAsignaturasCicloFormativo equals asigCiclo.IdAsignaturasCicloFormativo
                join usuario in _context.Usuarios on asigProf.IdProfesor equals usuario.IdUsuario
                join asignatura in _context.Asignaturas on asigCiclo.IdAsignatura equals asignatura.IdAsignatura
                join estudio in _context.Cicloformativos on asigCiclo.IdCiclo equals estudio.IdCiclo
                where asigProf.IdProfesor == idProfesor &&  estudio.IdCiclo == idEstudio && asignatura.IdAsignatura==idAsignatura
                select new AsignaturasprofesorModel()
                {
                    IdAsignatura = asignatura.IdAsignatura,
                    NombreAsignatura = asignatura.NombreAsignatura,
                    Curso = asignatura.Curso,
                    IdCiclo = estudio.IdCiclo,
                    Acronimo = estudio.Acronimo,
                    IdProfesor = asigProf.IdProfesor,
                    NombreUsuario = usuario.NombreUsuario,
                    ApellidosUsuario = usuario.ApellidosUsuario
                };

            return asignaturasprofesorListaDB.FirstOrDefault();
        }

        public async Task<AsignaturasprofesorModel> DeleteAsignaturasprofesor(int idProfesor, int idEstudio, int idAsignatura)
        {
            Scaffold.Asignaturascicloformativo asignaturascicloformativoDB = (from asigCiclo in _context.Asignaturascicloformativos
                                                                              where asigCiclo.IdCiclo == idEstudio && asigCiclo.IdAsignatura == idAsignatura
                                                                              select asigCiclo).FirstOrDefault()!;

            Scaffold.Asignaturasprofesor asignaturasprofesor = (from asigProf in _context.Asignaturasprofesors
                                                                where asigProf.IdProfesor == idProfesor && 
                                                                      asigProf.IdAsignaturasCicloFormativo == asignaturascicloformativoDB.IdAsignaturasCicloFormativo
                                                                select asigProf).FirstOrDefault()!;

            _context.Asignaturasprofesors.Remove(asignaturasprofesor);
            _context.SaveChanges();

            return new AsignaturasprofesorModel { IdAsignatura= idAsignatura, IdCiclo= idEstudio, IdProfesor=idProfesor };
        }

        public async Task<bool> ExistAsignaturasprofesor(int idProfesor, int idEstudio, int idAsignatura)
        {
            bool existe = false;

            Scaffold.Asignaturascicloformativo asignaturascicloformativoDB = (from asigCiclo in _context.Asignaturascicloformativos
                                                                              where asigCiclo.IdCiclo == idEstudio && asigCiclo.IdAsignatura == idAsignatura
                                                                              select asigCiclo).FirstOrDefault()!;

            IQueryable<Scaffold.Asignaturasprofesor> asignaturasprofesorListaDB = 
                from asigProf in _context.Asignaturasprofesors
                where asigProf.IdProfesor == idProfesor && 
                      asigProf.IdAsignaturasCicloFormativo == asignaturascicloformativoDB.IdAsignaturasCicloFormativo
                select asigProf;

            if (asignaturasprofesorListaDB.Count()!=0)
            {
                existe=true;
            }

            return existe;
        }

        public async Task<IEnumerable<AsignaturasprofesorModel>> ListAsignaturasAsignadasProfesor(int idProfesor)
        {
            IQueryable<AsignaturasprofesorModel> asignaturasprofesorListaDB =
                from asigProf in _context.Asignaturasprofesors
                join asigCiclo in _context.Asignaturascicloformativos on asigProf.IdAsignaturasCicloFormativo equals asigCiclo.IdAsignaturasCicloFormativo
                join usuario in _context.Usuarios on asigProf.IdProfesor equals usuario.IdUsuario
                join asignatura in _context.Asignaturas on asigCiclo.IdAsignatura equals asignatura.IdAsignatura
                join estudio in _context.Cicloformativos on asigCiclo.IdCiclo equals estudio.IdCiclo
                where asigProf.IdProfesor == idProfesor
                select new AsignaturasprofesorModel() 
                {
                    IdAsignatura = asignatura.IdAsignatura,
                    NombreAsignatura = asignatura.NombreAsignatura,
                    Curso = asignatura.Curso,
                    IdCiclo = estudio.IdCiclo,
                    Acronimo = estudio.Acronimo,
                    IdProfesor = asigProf.IdProfesor,
                    NombreUsuario = usuario.NombreUsuario,
                    ApellidosUsuario = usuario.ApellidosUsuario
                };

            return asignaturasprofesorListaDB.ToList();
        }

        public async Task<IEnumerable<HorarioModel>> ListHorariosProfesor(int idProfesor)
        {
            IQueryable<HorarioModel> listaHorariosProfesor =
                from asigProf in _context.Asignaturasprofesors
                join asigCiclo in _context.Asignaturascicloformativos
                    on asigProf.IdAsignaturasCicloFormativo equals asigCiclo.IdAsignaturasCicloFormativo
                join horario in _context.Horarios
                    on new { asigCiclo.IdAsignatura, asigCiclo.IdCiclo }
                    equals new { IdAsignatura = horario.IdAsignatura, IdCiclo = horario.IdEstudio }
                where asigProf.IdProfesor == idProfesor
                select new HorarioModel()
                {
                    IdHorario = horario.IdHorario,
                    IdAula = horario.IdAula,
                    IdDiaFranja = horario.IdDiaFranja,
                    IdAsignatura = horario.IdAsignatura,
                    IdEstudio = horario.IdEstudio,
                    ColorAsignatura = horario.ColorAsignatura
                };

            return listaHorariosProfesor.ToList();
        }

        public async Task<IEnumerable<AsignaturaModel>> ListAsignaturasProfesor(int idProfesor)
        {
            IQueryable<AsignaturaModel> listaAsignaturasProfesor =
                from asigProf in _context.Asignaturasprofesors
                join asigCiclo in _context.Asignaturascicloformativos
                    on asigProf.IdAsignaturasCicloFormativo equals asigCiclo.IdAsignaturasCicloFormativo
                join asignatura in _context.Asignaturas
                    on asigCiclo.IdAsignatura equals asignatura.IdAsignatura
                where asigProf.IdProfesor == idProfesor
                select new AsignaturaModel()
                {
                    IdAsignatura = asignatura.IdAsignatura,
                    NombreAsignatura = asignatura.NombreAsignatura,
                    Curso =asignatura.Curso
                };

            return listaAsignaturasProfesor.ToList();
        }

        public async Task<IEnumerable<AulaModel>> ListAulasProfesor(int idProfesor)
        {
            IQueryable<AulaModel> listaAulasProfesor =
                from asigProf in _context.Asignaturasprofesors
                join asigCiclo in _context.Asignaturascicloformativos
                    on asigProf.IdAsignaturasCicloFormativo equals asigCiclo.IdAsignaturasCicloFormativo
                join horario in _context.Horarios
                    on new { asigCiclo.IdAsignatura, asigCiclo.IdCiclo }
                    equals new { IdAsignatura = horario.IdAsignatura, IdCiclo = horario.IdEstudio }
                join aula in _context.Aulas on horario.IdAula equals aula.IdAula
                where asigProf.IdProfesor == idProfesor
                select new AulaModel()
                {
                    IdAula = aula.IdAula,
                    NumeroAula = aula.NumeroAula,
                    NombreAula = aula.NombreAula,
                    AforoMax = aula.AforoMax,
                    IdCentro = aula.IdCentro
                };

            return listaAulasProfesor.ToList();
        }

        public async Task<IEnumerable<CicloformativoModel>> ListEstudiosProfesor(int idProfesor)
        {
            IQueryable<CicloformativoModel> listaEstudiosProfesor =
                 from asigProf in _context.Asignaturasprofesors
                 join asigCiclo in _context.Asignaturascicloformativos
                     on asigProf.IdAsignaturasCicloFormativo equals asigCiclo.IdAsignaturasCicloFormativo
                 join cicloformativo in _context.Cicloformativos
                     on asigCiclo.IdCiclo equals cicloformativo.IdCiclo
                 where asigProf.IdProfesor == idProfesor
                 select new CicloformativoModel()
                 {
                     IdCiclo=cicloformativo.IdCiclo,
                     NombreCiclo=cicloformativo.NombreCiclo,
                     Acronimo=cicloformativo.Acronimo,
                     IdCentro=cicloformativo.IdCentro
                 };

            return listaEstudiosProfesor.ToList();
        }

        public async Task<bool> ExistHorarioEnConflictoProfesor(int idProfesor, int idEstudio, int idAsignatura)
        {
            bool existe = false;

            IQueryable<HorarioModel> listaHorariosAsignaturaEstudio =
                from horario in _context.Horarios
                where horario.IdEstudio == idEstudio && horario.IdAsignatura == idAsignatura
                orderby horario.IdDiaFranja ascending
                select new HorarioModel
                {
                    IdHorario = horario.IdHorario,
                    IdAula = horario.IdAula,
                    IdDiaFranja = horario.IdDiaFranja,
                    IdAsignatura = horario.IdAsignatura,
                    IdEstudio = horario.IdEstudio,
                    ColorAsignatura = horario.ColorAsignatura
                };

            List<HorarioModel> listaHorarioProfesor = await ListHorariosProfesor(idProfesor) as List<HorarioModel>;

            foreach(var horarioAsig in listaHorariosAsignaturaEstudio) 
            {
                existe = existe || listaHorarioProfesor.Exists(horarioProf => horarioProf.IdDiaFranja==horarioAsig.IdDiaFranja);
            }

            return existe;
        }
    }
}
