using ProyectoFinalDAMAgil.Models.Admin;
using ProyectoFinalDAMAgil.Scaffold;

namespace ProyectoFinalDAMAgil.Services.AsignaturasEstudios
{
    public class AsignaturasEstudiosService : IAsignaturasEstudiosService
    {
        private readonly DbappProyectoFinalContext _context;

        public AsignaturasEstudiosService(DbappProyectoFinalContext context)
        {
            _context=context;
        }

        public async Task<AsignaturasEstudiosModel> ReadAsignaturasEstudios(int idAsignaturasEstudios)
        {
            IQueryable<AsignaturasEstudiosModel> asignaturasEstudiosModelDB = from asignaturaEstudio in _context.Asignaturascicloformativos
                                                                              where asignaturaEstudio.IdAsignaturasCicloFormativo == idAsignaturasEstudios
                                                                              select new AsignaturasEstudiosModel 
                                                                              { 
                                                                                  IdAsignaturasEstudios = asignaturaEstudio.IdAsignaturasCicloFormativo,
                                                                                  IdAsignatura = asignaturaEstudio.IdAsignatura,
                                                                                  IdEstudio = asignaturaEstudio.IdCiclo
                                                                              };
            AsignaturasEstudiosModel asignaturasEstudiosModel = asignaturasEstudiosModelDB.FirstOrDefault();

            return asignaturasEstudiosModel;

        }

        public async Task<AsignaturasEstudiosModel> ReadIdAsignaturasEstudios(int idAsignatura, int idEstudio)
        {
            IQueryable<AsignaturasEstudiosModel> asignaturasEstudiosModelDB = from asignaturaEstudio in _context.Asignaturascicloformativos
                                                                              where asignaturaEstudio.IdAsignatura == idAsignatura && asignaturaEstudio.IdCiclo == idEstudio
                                                                              select new AsignaturasEstudiosModel
                                                                              {
                                                                                  IdAsignaturasEstudios = asignaturaEstudio.IdAsignaturasCicloFormativo,
                                                                                  IdAsignatura = asignaturaEstudio.IdAsignatura,
                                                                                  IdEstudio = asignaturaEstudio.IdCiclo
                                                                              };
            AsignaturasEstudiosModel asignaturasEstudiosModel = asignaturasEstudiosModelDB.FirstOrDefault();

            return asignaturasEstudiosModel;
        }
    }
}
