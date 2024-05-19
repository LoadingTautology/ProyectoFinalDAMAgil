using ProyectoFinalDAMAgil.Models.Admin;
using ProyectoFinalDAMAgil.Scaffold;

namespace ProyectoFinalDAMAgil.Services.Asignatura
{
    public class AsignaturaService : IAsignaturaService
    {
        private readonly DbappProyectoFinalContext _context;

        public AsignaturaService(DbappProyectoFinalContext context)
        {
            _context=context;
        }

        public Task<AsignaturaModel> CreateAsignatura(AsignaturaModel asignaturaModel)
        {
            throw new NotImplementedException();
        }

        public Task<AsignaturaModel> ReadAsignatura(int idAsignatura)
        {
            throw new NotImplementedException();
        }

        public Task<AsignaturaModel> UpdateAsignatura(AsignaturaModel asignaturaModel)
        {
            throw new NotImplementedException();
        }

        public Task<AsignaturaModel> DeleteAsignatura(AsignaturaModel asignaturaModel)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AsignaturaModel>> ListadoAsignatura(int idCiclo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistAsignatura(AsignaturaModel asignaturaModel)
        {
            throw new NotImplementedException();
        }

    }
}
