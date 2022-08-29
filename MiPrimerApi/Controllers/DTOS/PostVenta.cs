using MiPrimerApi.Model;

namespace MiPrimerApi.Controllers.DTOS
{
    public class PostVenta
    {
        public List<Producto> Productos { get; set; }
        public Venta Venta { get; set; }
    }
}
