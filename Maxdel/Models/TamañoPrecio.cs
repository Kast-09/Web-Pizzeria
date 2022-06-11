namespace Maxdel.Models
{
    public class TamañoPrecio
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public Productos Productos { get; set; }
        public string TamañoProducto { get; set; }
        public decimal Precio { get; set; }
    }
}
