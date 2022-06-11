using Maxdel.Models;

namespace Maxdel.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public int Estado { get; set; }
        public Estado EstadoFK { get; set; }
        public decimal? Monto { get; set; }
        public string? CodTracking { get; set; }
        public int? IdBoleta { get; set; }
        public Boleta boleta { get; set; }
    }
}
