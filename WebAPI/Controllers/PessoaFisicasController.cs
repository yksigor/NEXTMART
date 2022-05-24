using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contratos;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class PessoaFisicasController : ControllerBase
    {
        private readonly IPessoaServices pessoaServices;

        public PessoaFisicasController(IPessoaServices pessoaServices)
        {
            this.pessoaServices = pessoaServices;
        }

        public IActionResult Get()
        {
            var usuario = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier).Value;

            var pessoa = pessoaServices.GetPessoaFisicaByIdUsuario(int.Parse(usuario));

            return Ok(pessoa);
        }

        
    }
}