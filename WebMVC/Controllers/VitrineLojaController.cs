using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Repository.Data;
using WebMVC.Models;
using Services.Contratos;

namespace WebMVC.Controllers
{
    public class VitrineLojaController : Controller
    {
        private readonly LojaDBContext _context;
        private readonly ILoginServices _services;
        private readonly IProdutoServices produtoServices;

        public VitrineLojaController(LojaDBContext context, ILoginServices services, IProdutoServices produtoServices)
        {
            _context = context;
            _services = services;
            this.produtoServices = produtoServices;
        }

        public async Task<IActionResult> Index()
        {
            var vitrine = new VitrineLojaViewModel()
            {
                Produtos = await produtoServices.GetProdutosAsync(),
                CategoriaProdutos = await produtoServices.GetCategoriaProdutosAsync()
            };

            if (User.Identity.IsAuthenticated)
            {
                vitrine.User = await _services.BuscarUsuarioPeloIDAsync(Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value));
            }

            ViewBag.Categorias = _context.CategoriaProdutos.ToList();

            return View(vitrine);
        }

        public async Task<IActionResult> IndexByIdComerciante(int? id)
        {
            var vitrine = new VitrineLojaViewModel()
            {
                Produtos = produtoServices.GetProdutos(id.Value),
                CategoriaProdutos = await produtoServices.GetCategoriaProdutosAsync()
            };

            if (User.Identity.IsAuthenticated)
            {
                vitrine.User = await _services.BuscarUsuarioPeloIDAsync(Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value));
            }

            var categoria = produtoServices.GetCategoriaProdutoByIdComerciante(id.Value);

            ViewBag.Categorias = _context.CategoriaProdutos.ToList().Where<CategoriaProduto>(c => c.IdComerciante == id.Value);

            return View(vitrine);
        }

        public async Task<IActionResult> IndexByIdCategoria(int? id)
        {
            var vitrine = new VitrineLojaViewModel()
            {
                Produtos = produtoServices.GetProdutosByCategoria(id.Value),
                CategoriaProdutos = await produtoServices.GetCategoriaProdutosAsync()
            };

            if (User.Identity.IsAuthenticated)
            {
                vitrine.User = await _services.BuscarUsuarioPeloIDAsync(Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value));
            }

            ViewBag.Categorias = _context.CategoriaProdutos.ToList();

            return View(vitrine);
        }

        public ActionResult<List<Produto>> BuscarProduto(string nomeProduto)
        {
            var lista = _context.Produtos.Where(a => a.Nome.Contains(nomeProduto)).ToList();
            return new JsonResult(lista);
        }

        public async Task<IActionResult> ExibirBuscaProdutos(string nomeProduto)
        {
            nomeProduto = nomeProduto ?? "";

            var vitrine = new VitrineLojaViewModel()
            {
                Produtos = (await produtoServices.GetProdutosAsync()).Where(a => a.Nome.ToLower().Contains(nomeProduto.ToLower())).ToList(),
                CategoriaProdutos = await produtoServices.GetCategoriaProdutosAsync()
            };

            if (User.Identity.IsAuthenticated)
            {
                vitrine.User = await _services.GetUsuarioLogadoAsync(User);
            }

            ViewBag.Categorias = _context.CategoriaProdutos.ToList();

            return View("Index",vitrine);
        }

        public async Task<ActionResult> VerProduto(int? id)
        {
            if (id.HasValue)
            {
                var vitrine = new VitrineLojaViewModel()
                {
                    Produto = await produtoServices.GetProdutoAsync(id.Value)
                };

                vitrine.Produto.Categoria = produtoServices.GetCategoriaProduto(vitrine.Produto.IdCategoria);

                vitrine.Produto.Comerciante = produtoServices.GetComercianteProduto(vitrine.Produto.IdComerciante);

                if (vitrine.Produto != null) return View(vitrine.Produto); else return Redirect("/");
            }
            return Redirect("/");
        }
        
    }
}