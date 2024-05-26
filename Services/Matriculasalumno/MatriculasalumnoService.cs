using ProyectoFinalDAMAgil.Models.Admin;
using ProyectoFinalDAMAgil.Scaffold;

namespace ProyectoFinalDAMAgil.Services.Matriculasalumno
{
    public class MatriculasalumnoService : IMatriculasalumnoService
    {
        private readonly DbappProyectoFinalContext _context;

        public MatriculasalumnoService(DbappProyectoFinalContext context)
        {
            _context=context;
        }

        public async Task<MatriculasalumnoModel> CreateMatriculasalumno(int idAlumno, int idEstudio, int idAsignatura)
        {
            Scaffold.Asignaturascicloformativo asignaturascicloformativoDB = (from asigCiclo in _context.Asignaturascicloformativos
                                                                              where asigCiclo.IdCiclo == idEstudio && asigCiclo.IdAsignatura == idAsignatura
                                                                              select asigCiclo).FirstOrDefault()!;

            Scaffold.Matriculasalumno matriculasalumno = new Scaffold.Matriculasalumno()
            {
                IdAlumno=idAlumno,
                IdAsignaturasCicloFormativo = asignaturascicloformativoDB.IdAsignaturasCicloFormativo
            };
            _context.Matriculasalumnos.Add(matriculasalumno);
            _context.SaveChanges();

            return new MatriculasalumnoModel { IdAsignatura= idAsignatura, IdCiclo= idEstudio, IdAlumno=idAlumno };
        }

        public Task<MatriculasalumnoModel> ReadMatriculasalumno(int idAlumno, int idEstudio, int idAsignatura)
        {
            throw new NotImplementedException();
        }

        public async Task<MatriculasalumnoModel> DeleteMatriculasalumno(int idAlumno, int idEstudio, int idAsignatura)
        {
            Scaffold.Asignaturascicloformativo asignaturascicloformativoDB = (from asigCiclo in _context.Asignaturascicloformativos
                                                                              where asigCiclo.IdCiclo == idEstudio && asigCiclo.IdAsignatura == idAsignatura
                                                                              select asigCiclo).FirstOrDefault()!;

            Scaffold.Matriculasalumno matriculasalumno = (from matriAlumno in _context.Matriculasalumnos
                                                          where matriAlumno.IdAlumno == idAlumno &&
                                                                matriAlumno.IdAsignaturasCicloFormativo == asignaturascicloformativoDB.IdAsignaturasCicloFormativo
                                                          select matriAlumno).FirstOrDefault()!;

            _context.Matriculasalumnos.Remove(matriculasalumno);
            _context.SaveChanges();

            return new MatriculasalumnoModel { IdAsignatura= idAsignatura, IdCiclo= idEstudio, IdAlumno=idAlumno };
        }

        public async Task<bool> ExistMatriculasalumno(int idAlumno, int idEstudio, int idAsignatura)
        {
            bool existe = false;

            Scaffold.Asignaturascicloformativo asignaturascicloformativoDB = (from asigCiclo in _context.Asignaturascicloformativos
                                                                              where asigCiclo.IdCiclo == idEstudio && asigCiclo.IdAsignatura == idAsignatura
                                                                              select asigCiclo).FirstOrDefault()!;

            IQueryable<Scaffold.Matriculasalumno> matriculasalumnosListaDB =
                from matriAlumno in _context.Matriculasalumnos
                where matriAlumno.IdAlumno == idAlumno &&
                      matriAlumno.IdMatriculasAlumnos == asignaturascicloformativoDB.IdAsignaturasCicloFormativo
                select matriAlumno;

            if (matriculasalumnosListaDB.Count()!=0)
            {
                existe=true;
            }

            return existe;
        }

        public async Task<IEnumerable<MatriculasalumnoModel>> ListAsignaturasMatriculadasAlumno(int idAlumno)
        {
            IQueryable<MatriculasalumnoModel> asignaturasMatriculadasAlumnoListaDB =
                from matriAlumno in _context.Matriculasalumnos
                join asigCiclo in _context.Asignaturascicloformativos on matriAlumno.IdAsignaturasCicloFormativo equals asigCiclo.IdAsignaturasCicloFormativo
                join usuario in _context.Usuarios on matriAlumno.IdAlumno equals usuario.IdUsuario
                join asignatura in _context.Asignaturas on asigCiclo.IdAsignatura equals asignatura.IdAsignatura
                join estudio in _context.Cicloformativos on asigCiclo.IdCiclo equals estudio.IdCiclo
                where matriAlumno.IdAlumno == idAlumno
                select new MatriculasalumnoModel()
                {
                    IdAsignatura = asignatura.IdAsignatura,
                    NombreAsignatura = asignatura.NombreAsignatura,
                    Curso = asignatura.Curso,
                    IdCiclo = estudio.IdCiclo,
                    Acronimo = estudio.Acronimo,
                    IdAlumno = matriAlumno.IdAlumno,
                    NombreUsuario = usuario.NombreUsuario,
                    ApellidosUsuario = usuario.ApellidosUsuario
                };

            return asignaturasMatriculadasAlumnoListaDB.ToList();
        }

        public async Task<IEnumerable<HorarioModel>> ListHorariosAlumno(int idAlumno)
        {
            IQueryable<HorarioModel> listaHorariosAlumno =
                from matriAlum in _context.Matriculasalumnos
                join asigCiclo in _context.Asignaturascicloformativos
                    on matriAlum.IdAsignaturasCicloFormativo equals asigCiclo.IdAsignaturasCicloFormativo
                join horario in _context.Horarios
                    on new { asigCiclo.IdAsignatura, asigCiclo.IdCiclo }
                    equals new { IdAsignatura = horario.IdAsignatura, IdCiclo = horario.IdEstudio }
                where matriAlum.IdAlumno == idAlumno
                select new HorarioModel()
                {
                    IdHorario = horario.IdHorario,
                    IdAula = horario.IdAula,
                    IdDiaFranja = horario.IdDiaFranja,
                    IdAsignatura = horario.IdAsignatura,
                    IdEstudio = horario.IdEstudio,
                    ColorAsignatura = horario.ColorAsignatura
                };

            return listaHorariosAlumno.ToList();
        }

        public async Task<IEnumerable<AsignaturaModel>> ListAsignaturasAlumno(int idAlumno)
        {
            IQueryable<AsignaturaModel> listaAsignaturasAlumno =
                from matriAlumno in _context.Matriculasalumnos
                join asigCiclo in _context.Asignaturascicloformativos
                    on matriAlumno.IdAsignaturasCicloFormativo equals asigCiclo.IdAsignaturasCicloFormativo
                join asignatura in _context.Asignaturas
                    on asigCiclo.IdAsignatura equals asignatura.IdAsignatura
                where matriAlumno.IdAlumno == idAlumno
                select new AsignaturaModel()
                {
                    IdAsignatura = asignatura.IdAsignatura,
                    NombreAsignatura = asignatura.NombreAsignatura,
                    Curso =asignatura.Curso
                };

            return listaAsignaturasAlumno.ToList();
        }

        public async Task<IEnumerable<AulaModel>> ListAulasAlumno(int idAlumno)
        {
            IQueryable<AulaModel> listaAulasAlumno =
                from matriAlumno in _context.Matriculasalumnos
                join asigCiclo in _context.Asignaturascicloformativos
                    on matriAlumno.IdAsignaturasCicloFormativo equals asigCiclo.IdAsignaturasCicloFormativo
                join horario in _context.Horarios
                    on new { asigCiclo.IdAsignatura, asigCiclo.IdCiclo }
                    equals new { IdAsignatura = horario.IdAsignatura, IdCiclo = horario.IdEstudio }
                join aula in _context.Aulas on horario.IdAula equals aula.IdAula
                where matriAlumno.IdAlumno == idAlumno
                select new AulaModel()
                {
                    IdAula = aula.IdAula,
                    NumeroAula = aula.NumeroAula,
                    NombreAula = aula.NombreAula,
                    AforoMax = aula.AforoMax,
                    IdCentro = aula.IdCentro
                };

            return listaAulasAlumno.ToList();
        }

        public async Task<IEnumerable<CicloformativoModel>> ListEstudiosAlumno(int idAlumno)
        {
            IQueryable<CicloformativoModel> listaEstudiosAlumno =
                 from matriAlumno in _context.Matriculasalumnos
                 join asigCiclo in _context.Asignaturascicloformativos
                     on matriAlumno.IdAsignaturasCicloFormativo equals asigCiclo.IdAsignaturasCicloFormativo
                 join cicloformativo in _context.Cicloformativos
                     on asigCiclo.IdCiclo equals cicloformativo.IdCiclo
                 where matriAlumno.IdAlumno == idAlumno
                 select new CicloformativoModel()
                 {
                     IdCiclo=cicloformativo.IdCiclo,
                     NombreCiclo=cicloformativo.NombreCiclo,
                     Acronimo=cicloformativo.Acronimo,
                     IdCentro=cicloformativo.IdCentro
                 };

            return listaEstudiosAlumno.ToList();
        }

        public async Task<bool> ExistHorarioEnConflictoAlumno(int idAlumno, int idEstudio, int idAsignatura)
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

            List<HorarioModel> listaHorarioAlumno = await ListHorariosAlumno(idAlumno) as List<HorarioModel>;

            foreach (var horarioAsig in listaHorariosAsignaturaEstudio)
            {
                existe = existe || listaHorarioAlumno.Exists(horarioAlumno => horarioAlumno.IdDiaFranja==horarioAsig.IdDiaFranja);
            }

            return existe;
        }

    }
}
