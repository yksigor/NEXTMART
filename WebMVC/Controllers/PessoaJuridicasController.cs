using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Repository.Data;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Services.Contratos;

namespace WebMVC.Controllers
{
    public class PessoaJuridicasController : Controller
    {
        private readonly LojaDBContext _context;
        private readonly ILoginServices _services;
        private readonly IPessoaServices pessoaServices;

        public PessoaJuridicasController(LojaDBContext context, ILoginServices services, IPessoaServices pessoaServices)
        {
            _context = context;
            _services = services;
            this.pessoaServices = pessoaServices;
        }

        // GET: PessoaJuridicas
        public async Task<IActionResult> Index()
        {
            var lojaDBContext = _context.PessoasJuridicas.Include(p => p.Endereco).Include(p => p.Usuario);
            return View(await lojaDBContext.ToListAsync());
        }

        [HttpPost]
        public async Task<JsonResult> Cadastrar(PessoaJuridica pessoaJuridica)
        {
            //var state = ModelState;
            if (pessoaJuridica is null) return new JsonResult(NotFound());
            if (pessoaJuridica.Endereco is null || pessoaJuridica.Usuario is null) return new JsonResult(NotFound());

            var user = await _context.Usuarios.AddAsync(pessoaJuridica.Usuario);
            var endereco = await _context.Enderecos.AddAsync(pessoaJuridica.Endereco);
            await _context.SaveChangesAsync();

            pessoaJuridica.IdUsuario = user.Entity.Id;
            pessoaJuridica.IdEndereco = endereco.Entity.Id;

            pessoaJuridica.StatusComerciante = StatusComerciante.Ativo;

            var p = await _context.PessoasJuridicas.AddAsync(pessoaJuridica);
            await _context.SaveChangesAsync();

            try
            {
                var claims = _services.Autenticar(pessoaJuridica.Usuario);
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(new ClaimsPrincipal(identity),
                    new AuthenticationProperties { ExpiresUtc = DateTime.UtcNow.AddDays(8) });
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex.Message));
            }

            return new JsonResult(pessoaJuridica.Usuario);
        }

        // GET: PessoaJuridicas/Details/5
        public async Task<IActionResult> Details()
        {
            var user = await _services.BuscarUsuarioPeloIDAsync(Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value));
            var pessoaJuridica = await _context.PessoasJuridicas.Include(e => e.Endereco).Include(u => u.Usuario).FirstOrDefaultAsync(p => p.IdUsuario == user.Id);
            if (pessoaJuridica == null)
            {
                return NotFound();
            }

            return View(pessoaJuridica);
        }

        // GET: PessoaJuridicas/Create
        public IActionResult Create(Usuario usuario)
        {
            ViewBag.UF = Enum.GetValues(typeof(UF)).Cast<UF>().ToList();
            var pj = new PessoaJuridica();
            pj.Usuario = usuario;
            ViewData["IdEndereco"] = new SelectList(_context.Set<Endereco>(), "Id", "Id");
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Email");
            return View(pj);
        }

        // POST: PessoaJuridicas/Create
        [HttpPost]
        public async Task<IActionResult> Create(PessoaJuridica pessoaJuridica, Endereco endereco, Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Usuarios.Add(usuario);
                _context.Enderecos.Add(endereco);
                _context.SaveChanges();

                pessoaJuridica.IdEndereco = endereco.Id;

                pessoaJuridica.IdUsuario = usuario.Id;

                _context.PessoasJuridicas.Add(pessoaJuridica);
                await _context.SaveChangesAsync();

                return RedirectToAction("DashboardPessoaJuridica");
            }
            ViewData["IdEndereco"] = new SelectList(_context.Set<Endereco>(), "Id", "Id", pessoaJuridica.IdEndereco);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Email", pessoaJuridica.IdUsuario);
            ViewBag.UF = Enum.GetValues(typeof(UF)).Cast<UF>().ToList();
            return View();
        }

        //PessoaJuridicas/Dashboard
        public ActionResult DashboardPessoaJuridica()
        {
            return View();
        }

        // GET: PessoaJuridicas/Edit/5
        public ActionResult Edit()
        {
            var user = _services.BuscarUsuarioPeloIDAsync(Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value));
            if (user?.Result.Id != null)
            {
                var pj = _context.PessoasJuridicas.FirstOrDefault(p => p.IdUsuario == user.Result.Id);

                if (pj == null)
                {
                    return NotFound("Id não encontrado");
                }

                var endereco = _context.Enderecos.FirstOrDefault(e => e.Id == pj.IdEndereco);
                pj.Endereco = endereco;

                return View(pj);
            }

            return BadRequest("Id não pode ser vazio");
        }

        // POST: PessoaJuridicas/Edit/5
        [HttpPost]
        public IActionResult Edit(PessoaJuridica pessoaJuridica)
        {
            var user = _services.BuscarUsuarioPeloIDAsync(Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value));

            if (user?.Result.Id != null)
            {
                var pj = _context.PessoasJuridicas.FirstOrDefault(p => p.IdUsuario == user.Result.Id);

                if (user.Result.Id != pj.IdUsuario)
                {
                    return BadRequest("O id informado é diferente do id da Pessoa Jurídica");
                }

                if (pj is null)
                {
                    return NotFound("Pessoa não encontrada");
                }

                var endereco = _context.Enderecos.FirstOrDefault(e => e.Id == pessoaJuridica.IdEndereco);

                endereco.Logradouro = pessoaJuridica.Endereco.Logradouro;
                endereco.Municipio = pessoaJuridica.Endereco.Municipio;
                endereco.CEP = pessoaJuridica.Endereco.CEP;
                endereco.Complemento = pessoaJuridica.Endereco.Complemento;
                endereco.Bairro = pessoaJuridica.Endereco.Bairro;
                endereco.Numero = pessoaJuridica.Endereco.Numero;
                endereco.UF = pessoaJuridica.Endereco.UF;

                _context.Update(endereco);

                pj.Cnpj = pessoaJuridica.Cnpj;
                pj.NomeFantasia = pessoaJuridica.NomeFantasia;
                pj.RazaoSocial = pessoaJuridica.RazaoSocial;
                pj.Segmento = pessoaJuridica.Segmento;
                pj.Telefone = pessoaJuridica.Telefone;

                _context.Update(pj);
                _context.SaveChanges();

                return RedirectToAction("Index", "VitrineLoja");
            }
            return View(pessoaJuridica);
        }

        // GET: PessoaJuridicas/EditUsuario/5
        public async Task<IActionResult> EditUsuario()
        {
            var user = await _services.BuscarUsuarioPeloIDAsync(Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value));
            var pj = _context.PessoasJuridicas.FirstOrDefault(p => p.IdUsuario == user.Id);

            if (pj == null)
            {
                return NotFound();
            }

            return View(pj);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsuario(PessoaJuridica pessoaJuridica, string password, string confirmPass)
        {
            var user = _services.BuscarUsuarioPeloIDAsync(Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value));
            if (user.Result.Id == pessoaJuridica.Usuario.Id)
            {
                var usu = await _context.Usuarios.FirstOrDefaultAsync(us => us.Id == pessoaJuridica.Usuario.Id);
                usu.Username = pessoaJuridica.Usuario.Username;
                usu.Email = pessoaJuridica.Usuario.Email;

                if (password != null)
                {
                    if (password == confirmPass)
                    {
                        usu.Password = password;                        
                    }
                    else
                    {
                        return RedirectToAction();
                    }                    
                }

                _context.Update(usu);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "VitrineLoja");
            }
            return View(pessoaJuridica);
        }

        // GET: PessoaJuridicas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pessoaJuridica = await _context.PessoasJuridicas.Include(p => p.Endereco).Include(p => p.Usuario).FirstOrDefaultAsync(m => m.Id == id);
            if (pessoaJuridica == null)
            {
                return NotFound();
            }

            return View(pessoaJuridica);
        }

        // POST: PessoaJuridicas/Delete/5
        //Deleta o comerciante e retorna para a tela principal do Administrador
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pessoaJuridica = await _context.PessoasJuridicas.FindAsync(id);
            var endereco = await _context.Enderecos.FindAsync(pessoaJuridica.IdEndereco);
            var usuario = await _context.Usuarios.FindAsync(pessoaJuridica.IdUsuario);
            _context.PessoasJuridicas.Remove(pessoaJuridica);
            _context.Usuarios.Remove(usuario);
            _context.Enderecos.Remove(endereco);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Admin");
        }

        private bool PessoaJuridicaExists(int id)
        {
            return _context.PessoasJuridicas.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Carteira()
        {
            var user = await _services.GetUsuarioLogadoAsync(User);

            if (user != null)
            {
                var pessoa = await pessoaServices.GetPessoaJuridicaByIdAsync(user.Id);

                return View(pessoa);
            }
            return BadRequest("Não foi possivel encontrar o usuario!");
        }
    }
}