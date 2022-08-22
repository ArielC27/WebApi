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
        [HttpPost]
        public bool NuevaVenta([FromBody] PostVenta venta)
        {
            return VentaHandler.CreateNewSale(new Venta
            {
                Comentarios = venta.Comentarios
            });
        }
    }
}
