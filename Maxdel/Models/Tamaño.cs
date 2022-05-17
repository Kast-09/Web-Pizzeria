namespace Maxdel.Models
{
    public class Tamaño
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public Producto Producto { get; set; }
        public string TamañoProducto { get; set; }
    }
}
