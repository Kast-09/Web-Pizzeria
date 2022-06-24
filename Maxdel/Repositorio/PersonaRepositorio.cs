using Maxdel.DB;
using Maxdel.Models;

namespace Maxdel.Repositorio
{
    public interface IPersonaRepositorio
    {
        Usuario obtenerUsuarioId(int Id);
        List<Direcciones> obtenerDirecciones(int Id);
        void agregarDireccion(Direcciones direccion);
        void eliminarDireccion(int id);
        bool verificarContraseñaActual(int Id, string contraseñaActual);
        void actualizarBD();
        Usuario obtenerUsuarioString(string username);
        void actualizarContraseña(int Id, string contraseñaNueva);
    }
    public class PersonaRepositorio: IPersonaRepositorio
    {
        private readonly DbEntities _dbEntities;

        public PersonaRepositorio(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }

        public Usuario obtenerUsuarioId(int Id)
        {
            return _dbEntities.usuarios.First(o => o.Id == Id);
        }

        public List<Direcciones> obtenerDirecciones(int Id)
        {
            return _dbEntities.direcciones.Where(o => o.IdUsuario == Id).ToList();
        }

        public void agregarDireccion(Direcciones direccion)
        {
            _dbEntities.direcciones.Add(direccion);
            _dbEntities.SaveChanges();
        }

        public void eliminarDireccion(int id)
        {
            Direcciones direccion = _dbEntities.direcciones.First(o => o.Id == id);
            _dbEntities.direcciones.Remove(direccion);
            _dbEntities.SaveChanges();
        }

        public bool verificarContraseñaActual(int Id, string contraseñaActual)
        {
            return _dbEntities.usuarios.Any(x => x.Id == Id && x.Contraseña == contraseñaActual);
        }

        public void actualizarBD()
        {
            _dbEntities.SaveChanges();
        }

        public Usuario obtenerUsuarioString(string username)
        {
            return _dbEntities.usuarios.First(o => o.Correo == username);
        }

        public void actualizarContraseña(int Id, string contraseñaNueva)
        {
            Usuario user = _dbEntities.usuarios.First(o => o.Id == Id);
            user.Contraseña = contraseñaNueva;
            _dbEntities.SaveChanges();
        }
    }
}
