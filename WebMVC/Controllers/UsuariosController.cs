using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Repository.Data;

namespace WebMVC.Controllers
{
    //[Authorize("Bearer")]
    public class UsuariosController : Controller
    {
        private readonly LojaDBContext _context;

        public UsuariosController(LojaDBContext context)
        {
            _context = context;
        }

        public ActionResult Buscar()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> BuscarPorNome(Usuario usuario)
        {
            var lista = await _context.Usuarios.Where(a => a.Username.Contains(usuario.Username)).ToListAsync();
            return new JsonResult(lista);
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewBag.listaTipos = Enum.GetValues(typeof(TipoUsuario)).Cast<TipoUsuario>().ToList();

            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Email,Password,TipoUsuario")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                if(Convert.ToUInt32(usuario.TipoUsuario) == 0)
                {
                    return RedirectToAction("Create", "PessoaFisicas");
                }
                else
                {
                    if(Convert.ToUInt32(usuario.TipoUsuario) == 1)
                    {
                        return RedirectToAction("Create", "PessoaJuridicas");
                    }
                    else
                    {

                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Email,Password,TipoUsuario")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.listTipos = Enum.GetValues(typeof(TipoUsuario)).Cast<TipoUsuario>().ToList();
            return View(usuario);
        }

        // POST: Usuarios/EditarStatusAtivo/5
        [HttpPost]
        public async Task<IActionResult> EditarStatusAtivo(int id)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usuario.TipoUsuario == TipoUsuario.Comerciante)
            {
                var pj = await _context.PessoasJuridicas.FirstOrDefaultAsync(p => p.IdUsuario == usuario.Id);

                pj.StatusComerciante = StatusComerciante.Inativo;

                _context.PessoasJuridicas.Update(pj);
                await _context.SaveChangesAsync();
            }
            else
            {
                var pf = await _context.PessoasFisicas.FirstOrDefaultAsync(p => p.IdUsuario == usuario.Id);

                pf.StatusConsumidor = StatusConsumidor.Inativo;

                _context.PessoasFisicas.Update(pf);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Admin");
        }

        // POST: Usuarios/EditarStatusInativo/5
        [HttpPost]
        public async Task<IActionResult> EditarStatusInativo(int id)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);

            if (usuario.TipoUsuario == TipoUsuario.Comerciante)
            {
                var pj = await _context.PessoasJuridicas.FirstOrDefaultAsync(p => p.IdUsuario == usuario.Id);

                pj.StatusComerciante = StatusComerciante.Ativo;

                _context.PessoasJuridicas.Update(pj);
                await _context.SaveChangesAsync();
            }
            else
            {
                var pf = await _context.PessoasFisicas.FirstOrDefaultAsync(p => p.IdUsuario == usuario.Id);

                pf.StatusConsumidor = StatusConsumidor.Ativo;

                _context.PessoasFisicas.Update(pf);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Admin");
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            if (usuario.TipoUsuario == TipoUsuario.Comerciante)
            {
                var pj = await _context.PessoasJuridicas.FirstOrDefaultAsync(m => m.IdUsuario == usuario.Id);
                string status = pj.StatusComerciante.ToString();
                ViewBag.Status = status;
            }
            else
            {
                var pf = await _context.PessoasFisicas.FirstOrDefaultAsync(m => m.IdUsuario == usuario.Id);
                string status = pf.StatusConsumidor.ToString();
                ViewBag.Status = status;
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        //Deleta o usuário e retorna para a tela principal do Administrador
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Admin");
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
