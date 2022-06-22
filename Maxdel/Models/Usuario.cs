using Maxdel.Models;

namespace Maxdel.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public int IdRol { get; set; }
        public Roles Roles { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string? NroCelular { get; set; }
        public string? Correo { get; set; }
        public string? Contraseña { get; set; }
        public string? DNI { get; set; }
        public int? IdPreguntaSeguridad { get; set; }
        public PreguntaSeguridad? PreguntaSeguridad { get; set; }
        public string? RespuestaPS { get; set; }
        public List<Direcciones> Direcciones { get; set; }
        public List<Pedido> Pedidos { get; set; }
        public List<Boleta> Boletas { get; set; }
    }
}
