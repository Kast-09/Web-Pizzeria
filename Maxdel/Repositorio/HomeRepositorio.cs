using Maxdel.Models;
using Maxdel.DB;
using Microsoft.EntityFrameworkCore;

namespace Maxdel.Repositorio
{
    public interface IHomeRepositorio
    {
    }
    public class HomeRepositorio : IHomeRepositorio
    {
        private DbEntities _dbEntities;

        public HomeRepositorio(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }
    }
}
