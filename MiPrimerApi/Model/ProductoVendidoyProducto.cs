namespace MiPrimerApi.Model
{
    public class ProductoVendidoyProducto
    {

        public ProductoVendido ProductoVendido { get; set; }
        public Producto Producto { get; set; }

        public ProductoVendidoyProducto()
        {
            Producto Producto = new Producto();
            this.Producto = Producto;
            ProductoVendido ProductoVendido = new ProductoVendido();
            this.ProductoVendido = ProductoVendido;
        }
    }
}
