using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Contratos;

namespace WebMVC.Controllers
{
    public class CarrinhoController : Controller
    {
        CultureInfo ptBR = new CultureInfo("pt-BR");    

        private readonly IVendaServices vendaServices;
        private readonly IProdutoServices produtoServices;
        private readonly IPessoaServices pessoaServices;
        private readonly ILoginServices loginServices;
        public const string SessionCarrinho = "_Carrinho";

        public CarrinhoController(IVendaServices vendaServices, IProdutoServices produtoServices, IPessoaServices pessoaServices, ILoginServices loginServices)
        {
            this.produtoServices = produtoServices;
            this.vendaServices = vendaServices;
            this.pessoaServices = pessoaServices;
            this.loginServices = loginServices;
        }

        public IActionResult ObterJsonCarrinho()
        {
            return new JsonResult(ObterItensCarrinho(GetCarrinhoSessao()));
        }

        public IActionResult Index()
        {
            var itens = ObterItensCarrinho(GetCarrinhoSessao());

            return View(itens);
        }

        private Carrinho GetCarrinhoSessao()
        {
            var result = HttpContext.Session.GetString(SessionCarrinho) ?? "";
            return JsonConvert.DeserializeObject<Carrinho>(result);
        }

        private List<Item> ObterItensCarrinho(Carrinho carrinho)
        {
            if (carrinho is null) return new List<Item>();

            var itens = new List<Item>();

            carrinho.Itens.ForEach(item =>
            {
                var produto = produtoServices.GetProduto(item.IdProduto);           

                            
            itens.Add(new Item { ProdutoId = item.IdProduto, Quantidade = item.Quantidade, ValorUnidade = item.Valor, Produto = produto });

            });


            //var v = (String.Format(new CultureInfo("pt-BR"), "{0:C}", itens).ToList);

            return itens;
        }

        [HttpPost]
        public IActionResult AdicionarItem(ItemCarrinho item)
        {
            var carrinho = GetCarrinhoSessao();

            if(carrinho is null)
            {
                var itens = new List<ItemCarrinho> { item };
                HttpContext.Session.SetString(SessionCarrinho, JsonConvert.SerializeObject(new Carrinho { Itens = itens }));
            } else
            {
                carrinho = InsereItemNoCarrinho(item, carrinho);

                HttpContext.Session.SetString(SessionCarrinho, 
                    JsonConvert.SerializeObject(carrinho));
            }
            return new JsonResult(ObterItensCarrinho(carrinho));
        }

        [HttpPost]
        public IActionResult EditItem(ItemCarrinho item)
        {
            var carrinho = GetCarrinhoSessao();

            if (carrinho is null)
            {
                var itens = new List<ItemCarrinho> { item };
                HttpContext.Session.SetString(SessionCarrinho, JsonConvert.SerializeObject(new Carrinho { Itens = itens }));
            }
            else
            {
                carrinho = EditarItemCarrinho(item, carrinho);

                HttpContext.Session.SetString(SessionCarrinho,
                    JsonConvert.SerializeObject(carrinho));
            }
            return new JsonResult(ObterItensCarrinho(carrinho));
        }

        public async Task<IActionResult> EditarItem(int id)
        {
            var carrinho = GetCarrinhoSessao();

            if (carrinho is null)
            {
                return NotFound();
            }

            var itemCarrinho = carrinho.Itens.FirstOrDefault(p => p.IdProduto == id);
            var _item = new Item
            {
                ProdutoId = itemCarrinho.IdProduto,
                Produto = await produtoServices.GetProdutoAsync(itemCarrinho.IdProduto),
                Quantidade = itemCarrinho.Quantidade,
                ValorUnidade = itemCarrinho.Valor
            };

            return View(_item);
        }

        public IActionResult RemoverItem(int idProduto)
        {
            var carrinho = GetCarrinhoSessao();
            carrinho = RemoverItemCarrinho(idProduto, carrinho);
            HttpContext.Session.SetString(SessionCarrinho,
                    JsonConvert.SerializeObject(carrinho));
            return RedirectToAction("Index");
        }

        private Carrinho InsereItemNoCarrinho(ItemCarrinho itemCarrinho, Carrinho carrinho)
        {
            if(carrinho.Itens.Exists(a => a.IdProduto == itemCarrinho.IdProduto))
                carrinho.Itens.FirstOrDefault(a => a.IdProduto == itemCarrinho.IdProduto).Quantidade += itemCarrinho.Quantidade;
            else
                carrinho.Itens.Add(itemCarrinho);

            return carrinho;
        }

        private Carrinho EditarItemCarrinho(ItemCarrinho itemCarrinho, Carrinho carrinho)
        {
            if (carrinho.Itens.Exists(a => a.IdProduto == itemCarrinho.IdProduto))
                carrinho.Itens.FirstOrDefault(a => a.IdProduto == itemCarrinho.IdProduto).Quantidade = itemCarrinho.Quantidade;
            else
                carrinho.Itens.Add(itemCarrinho);

            return carrinho;
        }

        private Carrinho ReduzirItemCarrinho(int idProduto, Carrinho carrinho)
        {
            if (carrinho.Itens.Exists(a => a.IdProduto == idProduto))
                carrinho.Itens.FirstOrDefault(a => a.IdProduto == idProduto).Quantidade--;

            return carrinho;
        }

        private Carrinho RemoverItemCarrinho(int idProduto, Carrinho carrinho)
        {
            carrinho.Itens.Remove(carrinho.Itens.Find(a => a.IdProduto == idProduto));
            return carrinho;
        }

        public void ExcluirCarrinho()
        {
            HttpContext.Session.SetString(SessionCarrinho, "");
        }

        public IActionResult Remover()
        {
            ExcluirCarrinho();
            return Redirect("/");
        }

        [Authorize(Roles = "Consumidor")]
        public async Task<IActionResult> Finalizar()
        {
            var carrinho = GetCarrinhoSessao();
            var user = await loginServices.GetUsuarioLogadoAsync(User);

            var venda = new Venda()
            {
                Itens = ObterItensCarrinho(GetCarrinhoSessao()),
                DataVenda = DateTime.Now,
                IdUsuario = user.Id,
                Usuario = user,
                StatusVenda = StatusVenda.Aberta
            };

            venda = vendaServices.CreateVenda(venda);
            await produtoServices.AtualizarEstoque(venda.Itens);

            ExcluirCarrinho();

            return Redirect($"/Pedidos/Edit/{venda.Id}");
        }
    }
}