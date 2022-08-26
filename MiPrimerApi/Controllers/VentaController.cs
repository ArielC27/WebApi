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
        public List<Venta> Ventas ()
        {
            return VentaHandler.GetVentas();
        }

        [HttpPost(Name = "CreateVentas")]
        public bool NuevaVenta([FromBody] PostVenta venta)
        {
            return VentaHandler.CreateNewSale(new Venta
            {
                Comentarios = venta.Comentarios
            });
        }

        //[HttpDelete(Name = "DeleteVentas")]
        //public bool Venta EliminarVenta ()
        //{

        //}
    }
}
