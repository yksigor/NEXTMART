using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Repository.Data;
using WebMVC.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using Services.Contratos;
using Domain.Validate;
using WebMVC.Util;

namespace WebMVC.Controllers
{
    [Authorize(Roles = "Comerciante")]
    public class ProdutosController : Controller
    {
        private readonly LojaDBContext _context;
        private readonly ILoginServices _services;
        private readonly IProdutoServices produtoServices;
        private readonly IPessoaJuridicaServices pessoaJuridicaServices;
        private readonly IHostingEnvironment hostingEnvironment;

        public ProdutosController(LojaDBContext context, ILoginServices services, IProdutoServices produtoServices, IHostingEnvironment hostingEnvironment, IPessoaJuridicaServices pessoaJuridicaServices)
        {
            _context = context;
            _services = services;
            this.produtoServices = produtoServices;
            this.hostingEnvironment = hostingEnvironment;
            this.pessoaJuridicaServices = pessoaJuridicaServices;
        }

        // GET: Produtos
        public async Task<IActionResult> Index()
        {
            var idUsuario = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var comerciante = await pessoaJuridicaServices.GetPessoaJuridicaByIdUsuarioAsync(idUsuario);
            var produtos = produtoServices.GetProdutos(comerciante.Id);

            var categorias = await _context.CategoriaProdutos.ToListAsync();

            produtos.ForEach(p =>
            {
                p.Categoria = categorias.FirstOrDefault(c => c.Id == p.IdCategoria);
            });

            return View(produtos);
        }

        // GET: Produtos/Details/5
        public async Task<IActionResult> Details(int id)
        {
            int? idComerciante = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            if (idComerciante != null)
            {
                var produto = await produtoServices.GetProdutoAsync(id);

                if (produto == null)
                {
                    return NotFound("Produto não encontrado");
                }

                produto.Categoria = produtoServices.GetCategoriaProduto(produto.IdCategoria);

                return View(produto);
            }

            return BadRequest("Id não pode ser vazio");
        }

        // GET: Produtos/Create
        public async Task<IActionResult> Create()
        {
            var idUsuario = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var comerciante = await pessoaJuridicaServices.GetPessoaJuridicaByIdUsuarioAsync(idUsuario);
            var produto = new ProdutosViewModel();

            produto.DropDownCategorias = await produtoServices.GetCategoriaProdutosAsync(comerciante.Id);

            return View(produto);
        }

        // POST: Produtos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produto produto, List<IFormFile> arquivos)
        {
            var idUsuario = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var comerciante = await pessoaJuridicaServices.GetPessoaJuridicaByIdUsuarioAsync(idUsuario);

            if (ModelState.IsValid)
            {
                produto.IdComerciante = comerciante.Id;

                produtoServices.InsertOrUpdateProduto(produto.Id, produto);

                foreach (var source in arquivos)
                {
                    await _context.AddAsync(await FileUtil.InserirImagem(source, produto.Id, hostingEnvironment.WebRootPath));
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var produtoViewModel = new ProdutosViewModel();

            produtoViewModel.DropDownCategorias = await produtoServices.GetCategoriaProdutosAsync(comerciante.Id);
            produtoViewModel.Produto = produto;

            return View(produtoViewModel);
        }

        public async Task<IActionResult> RemoverImagem(int id)
        {
            if(await produtoServices.RemoverImagem(id, hostingEnvironment.WebRootPath))
                return Ok();
            else
                return BadRequest();
        }

        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var idUsuario = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var comerciante = await pessoaJuridicaServices.GetPessoaJuridicaByIdUsuarioAsync(idUsuario);

            var viewModel = new ProdutosViewModel()
            {
                Produto = await produtoServices.GetProdutoAsync(id.Value),
                DropDownCategorias = await produtoServices.GetCategoriaProdutosAsync(comerciante.Id),
                
            };

            viewModel.Produto.ImagensProduto = await produtoServices.GetImagensProdutoAsync(id.Value);


            foreach (var img in viewModel.Produto.ImagensProduto)
            {
                if(!FileUtil.ImagemExiste(hostingEnvironment.WebRootPath, img.NomeImagem))
                {
                    img.UrlImagem = "/img/not_found.jpg";
                }
            }

            if (viewModel.Produto.ImagensProduto == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Produto produto, List<IFormFile> arquivos)
        {
            int? idUsuario = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            if (idUsuario != null)
            {
                if (id != produto.Id)
                {
                    return NotFound("Id diferente do id do produto");
                }

                produtoServices.InsertOrUpdateProduto(id, produto);
                
                foreach (var source in arquivos)
                {
                    await _context.AddAsync(await FileUtil.InserirImagem(source, produto.Id, hostingEnvironment.WebRootPath));
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }


            return View(produto);
        }

        // GET: Produtos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("Produto não encontrado");
            }

            var produto = await produtoServices.GetProdutoAsync(id.Value);
            produto.Categoria = produtoServices.GetCategoriaProduto(produto.IdCategoria);
            produto.ImagensProduto = await produtoServices.GetImagensProdutoAsync(id.Value);

            if (produto == null)
            {
                return NotFound("Produto não existente");
            }

            return View(produto);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produto = await produtoServices.GetProdutoAsync(id);
            produtoServices.DeleteProduto(produto.IdComerciante, id);
            
            foreach (var imagem in produto.ImagensProduto)
            {
                await produtoServices.RemoverImagem(imagem.Id, hostingEnvironment.WebRootPath);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.Id == id);
        }
    }
}
