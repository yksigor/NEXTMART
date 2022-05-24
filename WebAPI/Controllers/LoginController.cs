using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Domain.Security;
using Services;
using Services.Contratos;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginServicesJWT _servicesJWT;
        private readonly ILoginServices _services;

        public LoginController(ILoginServicesJWT servicesJWT, ILoginServices services)
        {
            _servicesJWT = servicesJWT;
            _services = services;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<JsonResult> Post([FromBody]User user)
        {
            var usuario = await _services.GetUsuarioByUserAsync(user);
            if (usuario != null)
            {
                return new JsonResult(_servicesJWT.GerarToken(usuario));
            }

            return new JsonResult(new Token() { Authenticated = false, Message = "Falha ao autenticar" });
        }

        [HttpGet]
        [Authorize("Bearer")]
        public async Task<IActionResult> Get()
        {
            return new JsonResult((await _services.GetUsuarioLogadoAsync(User)).TipoUsuario);
        }
    }
}
