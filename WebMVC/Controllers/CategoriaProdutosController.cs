using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Repository.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Services.Contratos;

namespace WebMVC.Controllers
{
    [Authorize(Roles = "Comerciante")]
    public class CategoriaProdutosController : Controller
    {
        private readonly LojaDBContext _context;
        private readonly IProdutoServices produtoServices;
        private readonly IPessoaJuridicaServices pessoaJuridicaServices;
        private readonly ILoginServices _services;

        public CategoriaProdutosController(LojaDBContext context, ILoginServices services, IProdutoServices produtoServices, IPessoaJuridicaServices pessoaJuridicaServices)
        {
            _context = context;
            _services = services;
            this.produtoServices = produtoServices;
            this.pessoaJuridicaServices = pessoaJuridicaServices;
        }

        // GET: CategoriaProdutos
        public async Task<IActionResult> Index()
        {
            var idUsuario = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var comerciante = await pessoaJuridicaServices.GetPessoaJuridicaByIdUsuarioAsync(idUsuario);

            var categorias = await produtoServices.GetCategoriaProdutosAsync(comerciante.Id);

            return View(categorias);
        }

        // GET: CategoriaProdutos/Details/5
        public IActionResult Details(int id)
        {
            int? idComerciante = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            if (idComerciante != null)
            {
                var categoria = produtoServices.GetCategoriaProduto(id);

                if (categoria == null)
                {
                    return NotFound("Categoria não encontrada");
                }

                return View(categoria);
            }

            return BadRequest("Id não pode ser vazio");
        }

        // GET: CategoriaProdutos/Create
        public IActionResult Create()
        {            
            return View();
        }

        // POST: CategoriaProdutos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoriaProduto categoria)
        {
            var idUsuario = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var comerciante = await pessoaJuridicaServices.GetPessoaJuridicaByIdUsuarioAsync(idUsuario);

            if (ModelState.IsValid)
            {
                categoria.IdComerciante = comerciante.Id;

                produtoServices.InsertOrUpdateCategoria(categoria.Id, categoria);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: CategoriaProdutos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var idUsuario = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var comerciante = await pessoaJuridicaServices.GetPessoaJuridicaByIdUsuarioAsync(idUsuario);

            var categoria = produtoServices.GetCategoriaProduto(id.Value);

            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: CategoriaProdutos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CategoriaProduto categoria)
        {
            int? idUsuario = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            
            if (idUsuario != null)
            {
                if (id != categoria.Id)
                {
                    return NotFound("Id diferente do id da categoria");
                }

                produtoServices.InsertOrUpdateCategoria(id, categoria);

                return RedirectToAction("Index");
            }            
            return View(categoria);
        }

        // GET: CategoriaProdutos/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("Categoria não encontrada");
            }

            var categoria = produtoServices.GetCategoriaProduto(id.Value);

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: CategoriaProdutos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var categoria = produtoServices.GetCategoriaProduto(id);
            produtoServices.DeleteCategoria(categoria.IdComerciante, id);
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaProdutoExists(int id)
        {
            return _context.CategoriaProdutos.Any(e => e.Id == id);
        }
    }
}
