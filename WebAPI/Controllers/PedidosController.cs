using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contratos;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class PedidosController : ControllerBase
    {
        private readonly IVendaServices vendaServices;

        public PedidosController(IVendaServices vendaServices)
        {
            this.vendaServices = vendaServices;
        }

        public IActionResult Get()
        {
            var usuario = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier).Value;

            var pedidos = vendaServices.GetVendasByIdUsuario(int.Parse(usuario));

            return new JsonResult(pedidos);
        }
    }
}