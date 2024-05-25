using ProyectoFinalDAMAgil.Models.Admin;
using ProyectoFinalDAMAgil.Scaffold;

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

        public async Task<AsignaturasprofesorModel> DeleteAsignaturasprofesor(int idProfesor, int idEstudio, int idAsignatura)
        {
            Scaffold.Asignaturascicloformativo asignaturascicloformativoDB = (from asigCiclo in _context.Asignaturascicloformativos
                                                                              where asigCiclo.IdCiclo == idEstudio && asigCiclo.IdAsignatura == idAsignatura
                                                                              select asigCiclo).FirstOrDefault()!;

            Scaffold.Asignaturasprofesor asignaturasprofesor = (from asigProf in _context.Asignaturasprofesors
                                                                where asigProf.IdProfesor == idProfesor && asigProf.IdAsignaturasProfesor == asignaturascicloformativoDB.IdAsignaturasCicloFormativo
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
                where asigProf.IdProfesor == idProfesor && asigProf.IdAsignaturasProfesor == asignaturascicloformativoDB.IdAsignaturasCicloFormativo
                select asigProf;

            if (asignaturasprofesorListaDB.Count()!=0)
            {
                existe=true;
            }

            return existe;
        }

        public async Task<IEnumerable<AsignaturasprofesorModel>> ListAsignaturasprofesor(int idProfesor)
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

        public Task<AsignaturasprofesorModel> ReadAsignaturasprofesor(int idProfesor, int idEstudio, int idAsignatura)
        {
            throw new NotImplementedException();
        }
    }
}
