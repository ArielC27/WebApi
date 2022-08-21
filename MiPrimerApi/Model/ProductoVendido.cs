namespace MiPrimerApi.Model
{
    public class ProductoVendido
    {
        public int Id { get; set; }
        public int Stock { get; set; }
        public int IdVenta { get; set; }
        public int  IdProducto { get; set; }
        //public ProductoVendido()
        //{
        //    Producto producto = new Producto();
        //    IdProducto = producto;
        //}
    }
}
