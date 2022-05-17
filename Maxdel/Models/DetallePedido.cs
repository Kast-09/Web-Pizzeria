namespace Maxdel.Models
{
    public class DetallePedido
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public Producto Producto { get; set; }
        public int IdPedido { get; set; }
        public Pedido Pedido { get; set; }
        public int IdTamaño { get; set; }
        public Tamaño Tamaño { get; set; }
        public int IdPrecio { get; set; }
        public Precio Precio { get; set; }
        public int Cantidad { get; set; }
    }
}
