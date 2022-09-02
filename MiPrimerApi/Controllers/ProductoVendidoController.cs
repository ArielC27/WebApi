using Microsoft.AspNetCore.Mvc;
using MiPrimerApi.Model;
using MiPrimerApi.Repository;

namespace MiPrimerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductoVendidoController : ControllerBase
    {
        [HttpGet(Name = "GetProductosVendidos")]
        public List<ProductoVendidoyProducto> GetProductsSales(int IdUsuario)
        {
            return ProductoVendidoHandler.GetProductoVendido(IdUsuario);

        }
    }
}
