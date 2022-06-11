namespace Maxdel.Models
{
    public class Direcciones
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public string Direccion { get; set; }
        public string Referencia { get; set; }
    }
}
