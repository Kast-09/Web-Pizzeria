using Maxdel.DB;
using Maxdel.Models;
using Microsoft.EntityFrameworkCore;

namespace Maxdel.Repositorio
{
    public interface IVentasRepositorio
    {
        List<Boleta> listarBoletas();
        Usuario obtenerUsuario(string username);
    }
    public class VentasRepositorio : IVentasRepositorio
    {
        private readonly DbEntities _dbEntities;

        public VentasRepositorio(DbEntities dbEntities)
        {
            _dbEntities = dbEntities;
        }

        public List<Boleta> listarBoletas()
        {
            return _dbEntities.boletas
                                .Include("Pedidos")
                                .Where(o => o.Pedidos.Any(x => x.Estado == 5)).ToList();
        }

        public Usuario obtenerUsuario(string username)
        {
            return _dbEntities.usuarios.First(o => o.Correo == username);
        }
    }
}
