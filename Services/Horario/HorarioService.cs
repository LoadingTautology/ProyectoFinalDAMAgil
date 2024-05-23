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

        public async Task<HorarioModel> ReadHorario(int idAula, int idDiaFranja)
        {
            IQueryable<Scaffold.Horario> listaHorariosDB = from horario in _context.Horarios
                                                           where horario.IdAula == idAula && horario.IdDiaFranja == idDiaFranja
                                                           select new Scaffold.Horario
                                                           {
                                                               IdHorario = horario.IdHorario,
                                                               IdAula = horario.IdAula,
                                                               IdDiaFranja = horario.IdDiaFranja,
                                                               IdAsignatura = horario.IdAsignatura,
                                                               IdEstudio = horario.IdEstudio,
                                                               ColorAsignatura = horario.ColorAsignatura
                                                           };
            Scaffold.Horario horarioDB = listaHorariosDB.FirstOrDefault();

            HorarioModel horarioModel = null;

            if (horarioDB != null) 
            {
                horarioModel = new HorarioModel()
                {
                    IdHorario = horarioDB.IdHorario,
                    IdAula = horarioDB.IdAula,
                    IdDiaFranja = horarioDB.IdDiaFranja,
                    IdAsignatura = horarioDB.IdAsignatura,
                    IdEstudio = horarioDB.IdEstudio,
                    ColorAsignatura = horarioDB.ColorAsignatura
                };
            }

            return horarioModel;
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

        public async Task<bool> ExistHorarioSinAula(HorarioModel horario)
        {
            bool existe = false;
            IQueryable<HorarioModel> listaHorariosDB
                = from horarioBD in _context.Horarios
                  where horarioBD.IdDiaFranja == horario.IdDiaFranja && horarioBD.IdAsignatura == horario.IdAsignatura && horarioBD.IdEstudio == horario.IdEstudio
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
