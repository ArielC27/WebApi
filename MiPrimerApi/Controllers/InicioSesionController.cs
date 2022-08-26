using Microsoft.AspNetCore.Mvc;
using MiPrimerApi.Model;
using MiPrimerApi.Repository;

namespace MiPrimerApi.Controllers
{
    [ApiController]
    [Route ("[controller]")]
    public class InicioSesionController: ControllerBase
    {
        [HttpPost(Name = "InicioSesion")]
        public Usuario OpenSesion (string nombreUsuario, string contraseña)
        {
            return InicioSesion.IniciarSesion(nombreUsuario,contraseña);
        }
    }
}
