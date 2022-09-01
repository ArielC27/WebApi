using Microsoft.AspNetCore.Mvc;
using MiPrimerApi.Controllers.DTOS;
using MiPrimerApi.Model;
using MiPrimerApi.Repository;

namespace MiPrimerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VentaController : ControllerBase
    {
        [HttpGet(Name = "GetVentas")]
        public List<Venta> Ventas (int IdUsuario)
        {
            return VentaHandler.GetVentas(IdUsuario);
        }

        [HttpPost(Name = "CreateVentas")]
        public bool NuevaVenta([FromBody] PostVenta venta)
        {
            return VentaHandler.CreateNewSale(venta.Productos, venta.Venta);
         
        }

        [HttpDelete(Name = "DeleteVentas")]
        public bool EliminarVenta([FromBody] Venta venta)
        {
            return VentaHandler.EliminarVenta(venta);
        }
    }
}
