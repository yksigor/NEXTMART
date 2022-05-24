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
    [Authorize(Roles = "Consumidor")]
    public class PedidosController : Controller
    {
        private readonly IVendaServices vendaServices;
        private readonly ILoginServices loginServices;
        private readonly IPessoaServices pessoaServices;
        private readonly IProdutoServices produtoServices;

        public PedidosController(IVendaServices vendaServices, ILoginServices loginServices, IPessoaServices pessoaServices, IProdutoServices produtoServices)
        {
            this.vendaServices = vendaServices;
            this.pessoaServices = pessoaServices;
            this.loginServices = loginServices;
            this.produtoServices = produtoServices;
        }

        public async Task<IActionResult> Index()
        {
            var user = await loginServices.GetUsuarioLogadoAsync(User);
            var vendas = vendaServices.GetVendasByIdUsuario(user.Id);
            return View(vendas);
        }

        public async Task<IActionResult> Edit(int Id)
        {
            var venda = await vendaServices.GetVendaByIdAsync(Id);
            return View(venda);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var venda = await vendaServices.GetVendaByIdAsync(Id);

            if (venda.StatusVenda != StatusVenda.Concluida)
            {
                await produtoServices.Restock(venda.Itens);
                await vendaServices.RemoveAsync(Id);
            }
            else
            {
                await vendaServices.RemoveAsync(Id);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ItemUpdate(Item item)
        {
            if (item is null) return null;
            item = await vendaServices.UpdateItemAsync(item);
            var venda = await vendaServices.GetVendaByIdAsync(item.VendaId);

            return new JsonResult(venda.GetValorTotal());
        }

        [HttpPost]
        public async Task<IActionResult> ItemDelete(Item item)
        {
            if(await vendaServices.RemoveItemAsync(item.Id))
            {
                var venda = await vendaServices.GetVendaByIdAsync(item.VendaId);
                return new JsonResult(venda.GetValorTotal());
            }
            return BadRequest();
        }

        public async Task<IActionResult> Pagar(int idVenda)
        {
            var venda = await vendaServices.GetVendaByIdAsync(idVenda);

            venda = await vendaServices.PagarAsync(venda);

            if (venda.StatusVenda == StatusVenda.Aberta)
            {
                return View("FalhaPagamento", venda);
            }
            else
            {
                return View(venda);
            }            
        }
    }
}