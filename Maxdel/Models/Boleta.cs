namespace Maxdel.Models
{
    public class Boleta
    {
        public int Id { get; set; }
        public string NroBoleta { get; set; }
        public decimal MontoTotal { get; set; }
        public DateTime Fecha { get; set; }
        public string Direccion { get; set; }
        public string Referencia { get; set; }
    }
}
