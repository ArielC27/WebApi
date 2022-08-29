using Microsoft.AspNetCore.Mvc;
using MiPrimerApi.Controllers.DTOS;
using MiPrimerApi.Model;
using MiPrimerApi.Repository;

namespace MiPrimerApi.Controllers
{
    [ApiController]
    [Route ("[controller]")]
    public class InicioSesionController: ControllerBase
    {
        [HttpPost]
        public Usuario Login([FromBody] PostLogin login)
        {
            return InicioSesion.IniciarSesion(login.NombreUsuario, login.Contraseña);

        }
    }
}
