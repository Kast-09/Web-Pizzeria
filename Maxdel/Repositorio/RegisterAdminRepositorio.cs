using Maxdel.DB;
using Maxdel.Models;
using Maxdel.ViewModels;

namespace Maxdel.Repositorio
{
    public interface IRegisterAdminRepositorio
    {
        List<PreguntaSeguridad> listarPreguntas();
        void guardarUser(AgregarAdminClaseIntermedia account);
        Usuario obtenerUsuario(string username);
    }
    public class RegisterAdminRepositorio : IRegisterAdminRepositorio
    {
        private readonly DbEntities _dbEntities;

        public RegisterAdminRepositorio(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }

        public List<PreguntaSeguridad> listarPreguntas()
        {
            return _dbEntities.preguntaSeguridads.ToList(); ;
        }

        public void guardarUser(AgregarAdminClaseIntermedia account)
        {
            Usuario user = new Usuario();
            user.IdRol = 1;
            user.Nombre = account.Nombre;
            user.Apellido = account.Apellido;
            user.NroCelular = account.NroCelular;
            user.Correo = account.Correo;
            user.Contraseña = account.Contraseña;
            user.DNI = account.DNI;
            user.IdPreguntaSeguridad = account.IdPreguntaSeguridad;
            user.RespuestaPS = account.RespuestaPS;
            _dbEntities.usuarios.Add(user);
            _dbEntities.SaveChanges();
        }

        public Usuario obtenerUsuario(string username)
        {
            return _dbEntities.usuarios.First(o => o.Correo == username);
        }
    }
}
