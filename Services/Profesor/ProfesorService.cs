using ProyectoFinalDAMAgil.Models.Admin;
using ProyectoFinalDAMAgil.Scaffold;
using ProyectoFinalDAMAgil.Utilities;

namespace ProyectoFinalDAMAgil.Services.Profesor
{
    public class ProfesorService : IProfesorService
    {
        private readonly DbappProyectoFinalContext _context;

        public ProfesorService(DbappProyectoFinalContext context)
        {
            _context=context;
        }

        public async Task<ProfesorModel> CreateProfesor(ProfesorModel profesorModel, string password)
        {
            //Guardar Correo
            Scaffold.Correoelectronico correoelectronico = new Scaffold.Correoelectronico()
            {
                Email = profesorModel.Email,
                Clave = Encryptor.EncriptarClave(password)
            };
            _context.Correoelectronicos.Add(correoelectronico);
            _context.SaveChanges();

            //Guardar Usuario
            Scaffold.Usuario usuario = new Scaffold.Usuario()
            {
                NombreUsuario = profesorModel.NombreUsuario,
                ApellidosUsuario = profesorModel.ApellidosUsuario,
                Email = profesorModel.Email,
                Rol = "PROFESOR"
            };
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            //Guardar Profesor
            IQueryable<Scaffold.Usuario> usuarioDB = from user in _context.Usuarios
                                                     where user.Email == profesorModel.Email
                                                     select user;

            Scaffold.Profesor profesor = new Scaffold.Profesor()
            {
                IdProfesor = usuarioDB.FirstOrDefault().IdUsuario,
                Especialidad = profesorModel.Especialidad,
                IdCentro = profesorModel.IdCentro
            };
            _context.Profesors.Add(profesor);
            _context.SaveChanges();

            return profesorModel;
        }

        public async Task<ProfesorModel> ReadProfesor(int idProfesor)
        {
            IQueryable<ProfesorModel> profesoresListaDB
                = from profesor in _context.Profesors
                  join usuario in _context.Usuarios on profesor.IdProfesor equals usuario.IdUsuario
                  where profesor.IdProfesor == idProfesor
                  select new ProfesorModel()
                  {
                      NombreUsuario = usuario.NombreUsuario,
                      ApellidosUsuario = usuario.ApellidosUsuario,
                      Email = usuario.Email,
                      IdProfesor = profesor.IdProfesor,
                      Especialidad = profesor.Especialidad,
                      IdCentro = profesor.IdCentro
                  };

            return profesoresListaDB.FirstOrDefault();
        }

        public async Task<ProfesorModel> UpdateProfesor(ProfesorModel profesorModel)
        {
            //Actualizar Usuario
            Scaffold.Usuario usuarioDB = (from user in _context.Usuarios
                                          where user.Email == profesorModel.Email
                                          select user).FirstOrDefault()!;


            usuarioDB.NombreUsuario = profesorModel.NombreUsuario;
            usuarioDB.ApellidosUsuario = profesorModel.ApellidosUsuario;

            _context.Usuarios.Update(usuarioDB);
            _context.SaveChanges();

            //Actualizar Profesor
            Scaffold.Profesor profesorDB = (from profesor in _context.Profesors
                                            where profesor.IdProfesor == profesorModel.IdProfesor
                                            select profesor).FirstOrDefault()!;


            profesorDB.Especialidad = profesorModel.Especialidad;

            _context.Profesors.Update(profesorDB);
            _context.SaveChanges();

            return profesorModel;
        }

        public Task<ProfesorModel> DeleteProfesor(ProfesorModel profesorModel)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistProfesor(string emailProfesor, int idCentro)
        {
            //bool existe = false;

            //IQueryable<ProfesorModel> profesoresListaDB
            //    = from profesor in _context.Profesors
            //      join usuario in _context.Usuarios on profesor.IdProfesor equals usuario.IdUsuario
            //      where profesor.IdCentro == idCentro && usuario.Email == emailProfesor
            //      select new ProfesorModel()
            //      {
            //          NombreUsuario = usuario.NombreUsuario,
            //          ApellidosUsuario = usuario.ApellidosUsuario,
            //          Email = usuario.Email,
            //          IdProfesor = profesor.IdProfesor,
            //          Especialidad = profesor.Especialidad,
            //          IdCentro = profesor.IdCentro
            //      };

            //if (profesoresListaDB.Count()!=0)
            //{
            //    existe=true;
            //}

            //return existe;
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProfesorModel>> ListadoProfesores(int idCentro)
        {
            IQueryable<ProfesorModel> profesoresListaDB
                = from profesor in _context.Profesors
                  join usuario in _context.Usuarios on profesor.IdProfesor equals usuario.IdUsuario
                  where profesor.IdCentro == idCentro
                  select new ProfesorModel()
                  {
                      NombreUsuario = usuario.NombreUsuario,
                      ApellidosUsuario = usuario.ApellidosUsuario,
                      Email = usuario.Email,
                      IdProfesor = profesor.IdProfesor,
                      Especialidad = profesor.Especialidad,
                      IdCentro = profesor.IdCentro
                  };

            return profesoresListaDB.ToList();
        }

    }
}
