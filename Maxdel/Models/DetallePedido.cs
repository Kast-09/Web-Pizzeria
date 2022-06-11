namespace Maxdel.Models
{
    public class DetallePedido
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public Productos Producto { get; set; }
        public int IdPedido { get; set; }
        public Pedido Pedido { get; set; }
        public int IdTamañoPrecio { get; set; }
        public TamañoPrecio tamañoPrecio { get; set; }
        public int Cantidad { get; set; }
        public string TamañoProducto { get; set; }
        public decimal precio { get; set; }
    }
}
