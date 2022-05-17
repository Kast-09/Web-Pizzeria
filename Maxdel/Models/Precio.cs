namespace Maxdel.Models
{
    public class Precio
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public Producto Producto { get; set; }
        public int IdTamaño { get; set; }
        public Tamaño Tamaño { get; set; }
        public decimal precio { get; set; }
    }
}
