namespace Maxdel.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public Cliente Cliente { get; set; }
        public int Estado { get; set; }
        public Estado EstadoFK { get; set; }
        public string? CodTracking { get; set; }
    }
}
