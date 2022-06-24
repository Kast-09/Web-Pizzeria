using Maxdel.DB;
using Maxdel.Models;

namespace Maxdel.Repositorio
{
    public interface IHomeAdminRepositorio
    {
        Usuario obtenerUsuario(string username);
    }
    public class HomeAdminRepositorio: IHomeAdminRepositorio
    {
        private readonly DbEntities _dbEntities;

        public HomeAdminRepositorio(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }

        public Usuario obtenerUsuario(string username)
        {
            return _dbEntities.usuarios.First(o => o.Correo == username);
        }
    }
}
