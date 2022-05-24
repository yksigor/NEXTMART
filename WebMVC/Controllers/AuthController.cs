using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebMVC.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly LoginServices _services;

        public AuthController(LoginServices services)
        {
            _services = services;
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public JsonResult Post([FromBody]Usuario usuario)
        //{
        //    if (_services.Validate(usuario))
        //    {
        //        return new JsonResult(_services.GerarToken(usuario));
        //    }

        //    return new JsonResult(new Token() { Authenticated = false, Message = "Falha ao autenticar" });
        //}


    }
}