using Microsoft.AspNetCore.Mvc;
using MiPrimerApi.Repository;

namespace MiPrimerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NombreAppController : ControllerBase
    {
        [HttpGet(Name = "NameApp")]
        public string MostrarNombreApp()
        {
            return "Mi aplicacion Web";
        }
    }
}
