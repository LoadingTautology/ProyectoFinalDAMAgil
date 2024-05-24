using Microsoft.EntityFrameworkCore;
using ProyectoFinalDAMAgil.Models.Admin;
using ProyectoFinalDAMAgil.Scaffold;

namespace ProyectoFinalDAMAgil.Services.Horario
{
    public class HorarioService : IHorarioService
    {
        private readonly DbappProyectoFinalContext _context;

        public HorarioService(DbappProyectoFinalContext context)
        {
            _context=context;
        }

        public async Task<HorarioModel> CreateHorario(HorarioModel horario)
        {
            Scaffold.Horario horarioModel = new Scaffold.Horario()
            {
                IdAula = horario.IdAula,
                IdDiaFranja = horario.IdDiaFranja,
                IdAsignatura = horario.IdAsignatura,
                IdEstudio = horario.IdEstudio,
                ColorAsignatura = horario.ColorAsignatura
            };
            _context.Add(horarioModel);
            _context.SaveChanges();
            return horario;
        }

        public async Task<HorarioModel> ReadHorario(int idDiaFranja, int idAsignatura, int idEstudio)
        {
            IQueryable<HorarioModel> listaHorariosDB = 
                from horario in _context.Horarios
                where horario.IdDiaFranja == idDiaFranja && 
                      horario.IdAsignatura == idAsignatura &&
                      horario.IdEstudio == idEstudio
                select new HorarioModel
                {
                    IdHorario = horario.IdHorario,
                    IdAula = horario.IdAula,
                    IdDiaFranja = horario.IdDiaFranja,
                    IdAsignatura = horario.IdAsignatura,
                    IdEstudio = horario.IdEstudio,
                    ColorAsignatura = horario.ColorAsignatura
                };

            return listaHorariosDB.FirstOrDefault();
        }

        public async Task<HorarioModel> UpdateHorario(HorarioModel horario)
        {
            Scaffold.Horario horarioModel = new Scaffold.Horario()
            {
                IdHorario = horario.IdHorario,
                IdAula = horario.IdAula,
                IdDiaFranja = horario.IdDiaFranja,
                IdAsignatura = horario.IdAsignatura,
                IdEstudio = horario.IdEstudio,
                ColorAsignatura = horario.ColorAsignatura
            };

            _context.Update(horarioModel);
            _context.SaveChanges();
            return horario;
        }

        public async Task<HorarioModel> DeleteHorario(HorarioModel horario)
        {
            Scaffold.Horario horarioDB = new Scaffold.Horario()
            {
                IdHorario = horario.IdHorario,
                IdAula = horario.IdAula,
                IdDiaFranja = horario.IdDiaFranja,
                IdAsignatura = horario.IdAsignatura,
                IdEstudio = horario.IdEstudio,
                ColorAsignatura = horario.ColorAsignatura
            };

            _context.Horarios.Remove(horarioDB);
            _context.SaveChanges();
            return horario;
        }

        public async Task<bool> ExistHorario(int idAula, int idDiaFranja)
        {
            bool existe = false;
            IQueryable<HorarioModel> listaHorariosDB
                = from horarioBD in _context.Horarios
                  where horarioBD.IdAula == idAula && horarioBD.IdDiaFranja == idDiaFranja
                  select new HorarioModel
                  {
                      IdHorario = horarioBD.IdHorario,
                      IdAula = horarioBD.IdAula,
                      IdDiaFranja = horarioBD.IdDiaFranja,
                      IdAsignatura = horarioBD.IdAsignatura,
                      IdEstudio = horarioBD.IdEstudio,
                      ColorAsignatura = horarioBD.ColorAsignatura
                  };

            if (listaHorariosDB.Count()!=0)
            {
                existe=true;
            }

            return existe;
        }

        public async Task<bool> ExistHorario(int idDiaFranja, int idAsignatura, int idEstudio)
        {
            bool existe = false;
            IQueryable<HorarioModel> listaHorariosDB
                = from horarioBD in _context.Horarios
                  where horarioBD.IdDiaFranja == idDiaFranja &&
                        horarioBD.IdAsignatura == idAsignatura && horarioBD.IdEstudio == idEstudio
                  select new HorarioModel
                  {
                      IdHorario = horarioBD.IdHorario,
                      IdAula = horarioBD.IdAula,
                      IdDiaFranja = horarioBD.IdDiaFranja,
                      IdAsignatura = horarioBD.IdAsignatura,
                      IdEstudio = horarioBD.IdEstudio,
                      ColorAsignatura = horarioBD.ColorAsignatura
                  };

            if (listaHorariosDB.Count()!=0)
            {
                existe=true;
            }

            return existe;
        }

        public async Task<bool> ExistHorario(HorarioModel horario)
        {
            bool existe = false;
            IQueryable<HorarioModel> listaHorariosDB 
                =   from horarioBD in _context.Horarios
                    where horarioBD.IdDiaFranja == horario.IdDiaFranja && horarioBD.IdAula == horario.IdAula &&
                          horarioBD.IdAsignatura == horario.IdAsignatura && horarioBD.IdEstudio == horario.IdEstudio
                    select new HorarioModel
                    {
                        IdHorario = horarioBD.IdHorario,
                        IdAula = horarioBD.IdAula,
                        IdDiaFranja = horarioBD.IdDiaFranja,
                        IdAsignatura = horarioBD.IdAsignatura,
                        IdEstudio = horarioBD.IdEstudio,
                        ColorAsignatura = horarioBD.ColorAsignatura
                    };

            if (listaHorariosDB.Count()!=0)
            {
                existe=true;
            }

            return existe;
        }

        public async Task<IEnumerable<HorarioModel>> ListHorariosEstudio(int idEstudio)
        {
            IQueryable<HorarioModel> horarioListaDB = from horario in _context.Horarios
                                                      where horario.IdEstudio == idEstudio
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

            return horarioListaDB.ToList();
        }
        public async Task<IEnumerable<HorarioModel>> ListHorariosEstudioCursoAsignatura(int cursoAsignatura, int idEstudio) 
        {
            IQueryable<HorarioModel> horarioListaDB = from horario in _context.Horarios
                                                      join asignatura in _context.Asignaturas on horario.IdAsignatura equals asignatura.IdAsignatura
                                                      where horario.IdEstudio == idEstudio && asignatura.Curso == cursoAsignatura
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

            return horarioListaDB.ToList();

        }


        public async Task<IEnumerable<HorarioModel>> ListHorariosAsignaturaEstudio(int idAsignatura, int idEstudio)
        {
            IQueryable<HorarioModel> horarioListaDB = from horario in _context.Horarios
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

            return horarioListaDB.ToList();
        }

        public async Task<HorarioModel> CambiarColor(HorarioModel horario)
        {

            IQueryable<HorarioModel> horarioListaDB
                = from horarioBD in _context.Horarios
                    where horarioBD.IdAsignatura == horario.IdAsignatura && horarioBD.IdEstudio == horario.IdEstudio
                    select new HorarioModel
                    {
                        IdHorario = horarioBD.IdHorario,
                        IdAula = horarioBD.IdAula,
                        IdDiaFranja = horarioBD.IdDiaFranja,
                        IdAsignatura = horarioBD.IdAsignatura,
                        IdEstudio = horarioBD.IdEstudio,
                        ColorAsignatura = horarioBD.ColorAsignatura
                    };

            foreach (var item in horarioListaDB) 
            { 
                item.ColorAsignatura=horario.ColorAsignatura;
                this.UpdateHorario(item);
            }


            return horario;
        }


    }
}
