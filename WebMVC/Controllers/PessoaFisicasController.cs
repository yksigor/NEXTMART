using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Validate;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Services.Contratos;

namespace WebMVC.Controllers
{
    public class PessoaFisicasController : Controller
    {
        private readonly LojaDBContext _context;
        private readonly ILoginServices _services;
        private readonly IPessoaServices pessoaServices;

        public PessoaFisicasController(LojaDBContext context, ILoginServices services, IPessoaServices pessoaServices)
        {
            _context = context;
            _services = services;
            this.pessoaServices = pessoaServices;
        }

        [HttpPost]
        public async Task<JsonResult> Cadastrar(PessoaFisica pessoaFisica)
        {
            //var state = ModelState;
            if (pessoaFisica is null) return new JsonResult(NotFound());
            if (pessoaFisica.Endereco is null || pessoaFisica.Usuario is null) return new JsonResult(NotFound());

            var user = await _context.Usuarios.AddAsync(pessoaFisica.Usuario);
            var endereco = await _context.Enderecos.AddAsync(pessoaFisica.Endereco);
            await _context.SaveChangesAsync();

            pessoaFisica.IdUsuario = user.Entity.Id;
            pessoaFisica.IdEndereco = endereco.Entity.Id;

            pessoaFisica.StatusConsumidor = StatusConsumidor.Ativo;

            var p = await _context.PessoasFisicas.AddAsync(pessoaFisica);
            await _context.SaveChangesAsync();

            try
            {
                var claims = _services.Autenticar(pessoaFisica.Usuario);
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(new ClaimsPrincipal(identity),
                    new AuthenticationProperties { ExpiresUtc = DateTime.UtcNow.AddDays(8) });
            }
            catch (Exception ex)
            {
                return new JsonResult(NotFound(ex.Message));
            }

            return new JsonResult(pessoaFisica.Usuario);
        }

        // GET: PessoaFisicas
        public ActionResult Index()
        {
            var pf = _context.PessoasFisicas.Include(p => p.Usuario).Include(p => p.Endereco);
            return View(pf.ToList());
        }

        // GET: PessoaFisicas/Details/5
        public async Task<IActionResult> Details()
        {
            var user = await _services.BuscarUsuarioPeloIDAsync(Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value));
            var pessoaFisica = await _context.PessoasFisicas.Include(e => e.Endereco).Include(u => u.Usuario).FirstOrDefaultAsync(p => p.IdUsuario == user.Id);
            if (pessoaFisica == null)
            {
                return NotFound();
            }

            return View(pessoaFisica);
        }

        public ActionResult Create([Bind("Username,Email,Password,TipoUsuario")] Usuario usuario)
        {
            ViewBag.Sexo = Enum.GetValues(typeof(Sexo)).Cast<Sexo>().ToList();
            ViewBag.UF = Enum.GetValues(typeof(UF)).Cast<UF>().ToList();

            var pf = new PessoaFisica
            {
                Usuario = usuario
            };

            return View(pf);
        }

        // POST: PessoaFisicas/Create
        [HttpPost]
        public ActionResult Create([Bind("Endereco,Usuario,Cpf,NomeCompleto,DataNascimento,Cpf,Sexo,Telefone")] PessoaFisica pessoaFisica)
        {
            if (ModelState.IsValid)
            {
                var endereco = pessoaFisica.Endereco;
                var usuario = pessoaFisica.Usuario;

                var pessoa = _context.PessoasFisicas.FirstOrDefault(p => p.Cpf == pessoaFisica.Cpf);

                if (pessoa is null)
                {
                    _context.Enderecos.Add(endereco);
                    _context.Usuarios.Add(usuario);
                    _context.SaveChanges();

                    pessoaFisica.IdEndereco = endereco.Id;
                    pessoaFisica.IdUsuario = usuario.Id;

                    _context.PessoasFisicas.Add(pessoaFisica);
                    _context.SaveChanges();

                    var pessoaNova = _context.PessoasFisicas.LastOrDefault();

                    var pessoaNovaId = new PessoaFisica
                    {
                        Id = pessoaNova.Id
                    };
                    //return RedirectToAction("Index");
                    return RedirectToAction("DashboardPessoaFisica", pessoaNovaId);
                }
            }

            return View();
        }

        public JsonResult BuscarUsuario(string Username)
        {
            if (Username is null) return Json(new JsonResult("Digite um Usuário"));

            var usuario = _context.Usuarios.FirstOrDefault(a => a.Username == Username);

            if (usuario != null)
            {
                var erro = "Usuário já existe";

                return Json(new JsonResult(erro));
            }
            return Json(new JsonResult(null));
        }

        public JsonResult BuscarCPF(string CPF)
        {
            if (CPF is null) return Json(new JsonResult("Digite um CPF"));

            if (Validacao.Cpf(CPF))
            {
                var p = _context.PessoasFisicas.FirstOrDefault(a => a.Cpf.Equals(CPF));

                var erro = p is null ? null : "CPF já existe";

                return Json(new JsonResult(erro));
            }
            return Json(new JsonResult("CPF inválido"));
        }

        public async Task<IActionResult> Carteira()
        {
            var user = await _services.GetUsuarioLogadoAsync(User);

            if (user != null)
            {
                var pessoa = pessoaServices.GetPessoaFisicaByIdUsuarioAsync(user.Id).Result;

                return View(pessoa);
            }
            return BadRequest("Não foi possivel encontrar o usuario!");
        }

        [HttpPost]
        public IActionResult Carteira(PessoaFisica pessoa)
        {
            var p = _context.PessoasFisicas.FirstOrDefault(a => a.Id == pessoa.Id);
            p.Saldo = pessoa.Saldo;
            _context.PessoasFisicas.Update(p);
            _context.SaveChanges();

            ViewBag.Atualizado = "sim";

            return View(p);
        }

        // GET: PessoaFisicas/Edit/5
        public ActionResult Edit()
        {
            var user = _services.BuscarUsuarioPeloIDAsync(Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value));
            if (user?.Result.Id != null)
            {
                var pf = _context.PessoasFisicas.FirstOrDefault(p => p.IdUsuario == user.Result.Id);

                if (pf == null)
                {
                    return NotFound("Id não encontrado");
                }

                var endereco = _context.Enderecos.FirstOrDefault(u => u.Id == pf.IdEndereco);
                pf.Endereco = endereco;

                return View(pf);
            }

            return BadRequest("Id não pode ser vazio");
        }

        // POST: PessoaFisicas/Edit/5
        [HttpPost]
        public ActionResult Edit(PessoaFisica pessoaFisica)
        {
            var user = _services.BuscarUsuarioPeloIDAsync(Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value));

            if (user?.Result.Id != null)
            {
                var pf = _context.PessoasFisicas.FirstOrDefault(p => p.IdUsuario == user.Result.Id);

                if (user.Result.Id != pf.IdUsuario)
                {
                    return BadRequest("O id informado é diferente do id da Pessoa Física");
                }

                if (pf is null)
                {
                    return NotFound("Pessoa não encontrada");
                }

                var endereco = _context.Enderecos.FirstOrDefault(e => e.Id == pessoaFisica.IdEndereco);

                endereco.Logradouro = pessoaFisica.Endereco.Logradouro;
                endereco.Municipio = pessoaFisica.Endereco.Municipio;
                endereco.CEP = pessoaFisica.Endereco.CEP;
                endereco.Complemento = pessoaFisica.Endereco.Complemento;
                endereco.Bairro = pessoaFisica.Endereco.Bairro;
                endereco.Numero = pessoaFisica.Endereco.Numero;
                endereco.UF = pessoaFisica.Endereco.UF;

                _context.Update(endereco);

                pf.Cpf = pessoaFisica.Cpf;
                pf.DataNascimento = pessoaFisica.DataNascimento;
                pf.NomeCompleto = pessoaFisica.NomeCompleto;
                pf.Sexo = pessoaFisica.Sexo;
                pf.Telefone = pessoaFisica.Telefone;

                _context.Update(pf);
                _context.SaveChanges();

                return RedirectToAction("Index", "VitrineLoja");
            }
            return View(pessoaFisica);
        }

        public async Task<IActionResult> EditUsuario()
        {
            var user = await _services.BuscarUsuarioPeloIDAsync(Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value));
            var pf = _context.PessoasFisicas.FirstOrDefault(p => p.IdUsuario == user.Id);

            if (pf == null)
            {
                return NotFound();
            }

            return View(pf);
        }

        [HttpPost]
        public ActionResult EditUsuario(PessoaFisica pessoaFisica, string password, string confirmPass)
        {
            var user = _services.BuscarUsuarioPeloIDAsync(Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value));

            if (user.Result.Id == pessoaFisica.Usuario.Id)
            {
                var usu = _context.Usuarios.FirstOrDefault(us => us.Id == pessoaFisica.Usuario.Id);
                usu.Username = pessoaFisica.Usuario.Username;
                usu.Email = pessoaFisica.Usuario.Email;

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
                _context.SaveChanges();

                return RedirectToAction("Index", "VitrineLoja");
            }
            return View(pessoaFisica);
        }

        // GET: PessoaFisicas/Delete/5
        public async  Task<ActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var pf = await pessoaServices.GetPessoaFisicaAsync(id.Value);

                if (pf is null) return NotFound("Pessoa não encontrada");

                return View(pf);
            }
            return BadRequest("Id não pode ser vazio");
        }

        // POST: PessoaFisicas/Delete/5
        //Deleta o consumidor e retorna para a tela principal do Administrador
        [HttpPost]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var pf = _context.PessoasFisicas.FirstOrDefault(p => p.Id == id);
            var usuario = _context.Usuarios.FirstOrDefault(us => us.Id == id);
            var endereco = _context.Enderecos.FirstOrDefault(en => en.Id == id);

            _context.PessoasFisicas.Remove(pf);
            _context.Usuarios.Remove(usuario);
            _context.Enderecos.Remove(endereco);

            _context.SaveChanges();

            return RedirectToAction("Index", "Admin");
        }

        // PessoaFisicas/DashboardPessoaFisica/
        public ActionResult DashboardPessoaFisica()
        {
            return View();
        }
    }
}