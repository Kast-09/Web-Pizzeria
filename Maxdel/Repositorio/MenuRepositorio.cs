using Maxdel.DB;
using Maxdel.Models;

namespace Maxdel.Repositorio
{
    public interface IMenuRepositorio
    {
        List<Productos> listarProductos();
    }
    public class MenuRepositorio: IMenuRepositorio
    {
        private readonly DbEntities _dbEntities;

        public MenuRepositorio(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }

        public List<Productos> listarProductos()
        {
            return _dbEntities.Productos.ToList();
        }
    }
}
