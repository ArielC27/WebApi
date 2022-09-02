using Microsoft.AspNetCore.Mvc;
using MiPrimerApi.Controllers.DTOS;
using MiPrimerApi.Model;
using MiPrimerApi.Repository;

namespace MiPrimerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductoController : ControllerBase
    {
        [HttpGet(Name = "GetProducts")]
        public List<Producto> GetProductos()
        {
            return ProductoHandler.GetProducts();
        }

        [HttpPost(Name = "CreateProduct")]
        public bool CrearProducto([FromBody] PostProducto producto)
        {
            return ProductoHandler.CreateProduct(new Producto
            {
                Descripciones = producto.Descripciones,
                Costo = producto.Costo,
                PrecioVenta = producto.PrecioVenta,
                Stock = producto.Stock,
                IdUsuario = producto.IdUsuario,
            });
        }

        [HttpPut(Name = "UpdateProduct")]
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

        [HttpDelete(Name = "DeleteProducts")]
        public bool EliminarProducto([FromBody] int id)
        {
            return ProductoHandler.DeleteProduct(id);
        }
    }
}
