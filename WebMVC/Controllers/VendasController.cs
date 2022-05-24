using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contratos;

namespace WebMVC.Controllers
{
    [Authorize(Roles = "Comerciante")]
    public class VendasController : Controller
    {
        private readonly IVendaServices vendaServices;
        private readonly ILoginServices loginServices;

        public VendasController(IVendaServices vendaServices, ILoginServices loginServices)
        {
            this.vendaServices = vendaServices;
            this.loginServices = loginServices;
        }

        public async Task<IActionResult> Index()
        {
            var user = await loginServices.GetUsuarioLogadoAsync(User);
            var pessoa = await loginServices.RetornaPessoaPeloUsuarioAsync(user);

            var itensvendidos = await vendaServices.ItensVendidos(pessoa as PessoaJuridica);

            return View(itensvendidos);
        }
    }
}