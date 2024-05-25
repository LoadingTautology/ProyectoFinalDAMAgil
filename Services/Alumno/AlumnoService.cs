using ProyectoFinalDAMAgil.Models.Admin;
using ProyectoFinalDAMAgil.Scaffold;
using ProyectoFinalDAMAgil.Utilities;

namespace ProyectoFinalDAMAgil.Services.Alumno
{
    public class AlumnoService : IAlumnoService
    {
        private readonly DbappProyectoFinalContext _context;

        public AlumnoService(DbappProyectoFinalContext context)
        {
            _context=context;
        }

        public async Task<AlumnoModel> CreateAlumno(AlumnoModel alumnoModel, string password)
        {
            //Guardar Correo
            Scaffold.Correoelectronico correoelectronico = new Scaffold.Correoelectronico()
            {
                Email = alumnoModel.Email,
                Clave = Encryptor.EncriptarClave(password)
            };
            _context.Correoelectronicos.Add(correoelectronico);
            _context.SaveChanges();

            //Guardar Usuario
            Scaffold.Usuario usuario = new Scaffold.Usuario()
            {
                NombreUsuario = alumnoModel.NombreUsuario,
                ApellidosUsuario = alumnoModel.ApellidosUsuario,
                Email = alumnoModel.Email,
                Rol = "ALUMNO"
            };
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            //Guardar Alumno
            IQueryable<Scaffold.Usuario> usuarioDB = from user in _context.Usuarios
                                                     where user.Email == alumnoModel.Email
                                                     select user;

            Scaffold.Alumno alumno = new Scaffold.Alumno()
            {
                IdAlumno = usuarioDB.FirstOrDefault().IdUsuario,
                FechaDeNacimiento = alumnoModel.FechaDeNacimiento,
                IdCentro = alumnoModel.IdCentro
            };
            _context.Alumnos.Add(alumno);
            _context.SaveChanges();

            return alumnoModel;
        }

        public async Task<AlumnoModel> ReadAlumno(int idAlumno)
        {
            IQueryable<AlumnoModel> alumnosListaDB
                = from alumno in _context.Alumnos
                  join usuario in _context.Usuarios on alumno.IdAlumno equals usuario.IdUsuario
                  where alumno.IdAlumno == idAlumno
                  select new AlumnoModel()
                  {
                      NombreUsuario = usuario.NombreUsuario,
                      ApellidosUsuario = usuario.ApellidosUsuario,
                      Email = usuario.Email,
                      IdAlumno = alumno.IdAlumno,
                      FechaDeNacimiento = alumno.FechaDeNacimiento,
                      IdCentro = alumno.IdCentro

                  };

            return alumnosListaDB.FirstOrDefault();
        }

        public async Task<AlumnoModel> UpdateAlumno(AlumnoModel alumnoModel)
        {
            //Actualizar Usuario
            Scaffold.Usuario usuarioDB = (from user in _context.Usuarios
                                            where user.Email == alumnoModel.Email
                                            select user).FirstOrDefault()!;


            usuarioDB.NombreUsuario = alumnoModel.NombreUsuario;
            usuarioDB.ApellidosUsuario = alumnoModel.ApellidosUsuario;

            _context.Usuarios.Update(usuarioDB);
            _context.SaveChanges();

            //Actualizar Alumno
            Scaffold.Alumno alumnoDB = (from alumno in _context.Alumnos
                                        where alumno.IdAlumno == alumnoModel.IdAlumno
                                        select alumno).FirstOrDefault()!;


            alumnoDB.FechaDeNacimiento = alumnoModel.FechaDeNacimiento;

            _context.Alumnos.Update(alumnoDB);
            _context.SaveChanges();

            return alumnoModel;
        }

        public Task<AlumnoModel> DeleteAlumno(AlumnoModel alumnoModel)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistAlumno(string emailAlumno, int idCentro)
        {
            //bool existe = false;

            //IQueryable<AlumnoModel> alumnosListaDB
            //    = from alumno in _context.Alumnos
            //      join usuario in _context.Usuarios on alumno.IdAlumno equals usuario.IdUsuario
            //      where alumno.IdCentro == idCentro && usuario.Email == emailAlumno
            //      select new AlumnoModel()
            //      {
            //          NombreUsuario = usuario.NombreUsuario,
            //          ApellidosUsuario = usuario.ApellidosUsuario,
            //          Email = usuario.Email,
            //          IdAlumno = alumno.IdAlumno,
            //          FechaDeNacimiento = alumno.FechaDeNacimiento,
            //          IdCentro = alumno.IdCentro

            //      };

            //if (alumnosListaDB.Count()!=0)
            //{
            //    existe=true;
            //}

            //return existe;
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AlumnoModel>> ListadoAlumnos(int idCentro)
        {
            IQueryable<AlumnoModel> alumnosListaDB
                = from alumno in _context.Alumnos
                  join usuario in _context.Usuarios on alumno.IdAlumno equals usuario.IdUsuario
                  where alumno.IdCentro == idCentro
                  select new AlumnoModel()
                  {
                      NombreUsuario = usuario.NombreUsuario,
                      ApellidosUsuario = usuario.ApellidosUsuario,
                      Email = usuario.Email,
                      IdAlumno = alumno.IdAlumno,
                      FechaDeNacimiento = alumno.FechaDeNacimiento,
                      IdCentro = alumno.IdCentro

                  };

            return alumnosListaDB.ToList();
        }


    }
}
