namespace Maxdel.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public int IdTipo { get; set; }
        public Tipo Tipo { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string? NroCelular { get; set; }
        public string? Correo { get; set; }
        public string? Contraseña { get; set; }
        public string? DNI { get; set; }
    }
}
