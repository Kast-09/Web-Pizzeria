using Maxdel.DB;
using Maxdel.Models;

namespace Maxdel.Repositorio
{
    public interface IAuthRepositorio
    {
        bool verificar(string correo, string contraseña);
        Usuario obtenerUsuario(string correo, string contraseña);
        List<PreguntaSeguridad> obtenerPreguntas();
        int contarUsuarios();
        int obtenerUltimoUsuaro();
        Usuario obtenerUsuario(string correo);
        bool verificarPreguntaSeguridad(int Id, int IdPreguntaSeguridad, string RespuestaPS);
        void actualizarUsuario(int Id, string contraseña);
        void registrar(Usuario user, Direcciones direcciones);
    }
    public class AuthRepositorio : IAuthRepositorio
    {
        private readonly DbEntities _dbEntities;

        public AuthRepositorio(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }

        public bool verificar(string correo, string contraseña)
        {
            return _dbEntities.usuarios.Any(x => x.Correo == correo && x.Contraseña == contraseña);
        }

        public Usuario obtenerUsuario(string correo, string contraseña)
        {
            return _dbEntities.usuarios.First(x => x.Correo == correo && x.Contraseña == contraseña);
        }

        public List<PreguntaSeguridad> obtenerPreguntas()
        {
            return _dbEntities.preguntaSeguridads.ToList();
        }

        public int contarUsuarios()
        {
            return _dbEntities.usuarios.Count();
        }

        public int obtenerUltimoUsuaro()
        {
            return _dbEntities.usuarios.OrderBy(o => o.Id).Last().Id + 1;
        }

        public void registrar(Usuario user, Direcciones direcciones)
        {
            _dbEntities.usuarios.Add(user);
            _dbEntities.direcciones.Add(direcciones);
            _dbEntities.SaveChanges();
        }

        public Usuario obtenerUsuario(string correo)
        {
            return _dbEntities.usuarios.FirstOrDefault(o => o.Correo == correo);
        }

        public bool verificarPreguntaSeguridad(int Id, int IdPreguntaSeguridad, string RespuestaPS)
        {
            return _dbEntities.usuarios.Any(o => o.Id == Id
                       && o.IdPreguntaSeguridad == IdPreguntaSeguridad
                       && o.RespuestaPS == RespuestaPS);
        }

        public void actualizarUsuario(int Id, string contraseña)
        {
            Usuario user = _dbEntities.usuarios.First(o => o.Id == Id);
            user.Contraseña = contraseña;
            _dbEntities.SaveChanges();
        }
    }
}
