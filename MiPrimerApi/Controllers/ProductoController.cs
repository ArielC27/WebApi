using Microsoft.AspNetCore.Mvc;
using MiPrimerApi.Model;
using MiPrimerApi.Repository;

namespace MiPrimerApi.Controllers
{
    [ApiController]
    [Route ("[controller]")]
    public class ProductoController : ControllerBase
    {
        [HttpGet (Name= "GetProductos")]
        public List<Producto> GetProductos()
        {
            return ProductoHandler.GetProductos();
        }

        [HttpPut]
        public bool ModificarProducto([FromBody] Producto producto)
        {
            return ProductoHandler.UpdateProduct(new Producto
            {
                Id = producto.Id,
                Descripciones = producto.Descripciones,
                Costo = producto.Costo,
                PrecioVenta = producto.PrecioVenta,
                Stock = producto.Stock,
                IdUsuario = producto.IdUsuario,
            });
        }

        [HttpPost]
        public bool CrearProducto([FromBody] Producto producto)
        {
            return ProductoHandler.CreateProduct(new Producto
            {
                Descripciones=producto.Descripciones,
                Costo=producto.Costo,
                PrecioVenta=producto.PrecioVenta,
                Stock=producto.Stock,
                IdUsuario=producto.IdUsuario,
            });
        }

        [HttpDelete]
        public bool  EliminarProducto([FromBody] int id)
        {
            return ProductoHandler.DeleteProduct(id);
        }
    }
}
