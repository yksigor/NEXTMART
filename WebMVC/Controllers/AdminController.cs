using System.Linq;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Repository.Data;
using WebMVC.Models;
using Services.Contratos;
using System.Threading.Tasks;

namespace WebMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly LojaDBContext context;
        private readonly IVendaServices vendaServices;
        private readonly ILoginServices loginServices;

        public AdminController(LojaDBContext context, IVendaServices vendaServices, ILoginServices loginServices)
        {
            this.context = context;
            this.vendaServices = vendaServices;
            this.loginServices = loginServices;
        }

        // GET: Admin
        public ActionResult Index()
        {
            var model = new AdministradorViewModel();
            model.Tarifas = context.Tarifas.ToList();
            return View(model);
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarifa = context.Tarifas.Find(id);

            if (tarifa == null)
            {
                return NotFound();
            }

            return View(tarifa);
        }

        [HttpPost]
        public ActionResult CadastroTarifa(Tarifa tarifa)
        {
            if (ModelState.IsValid)
            {
                context.Tarifas.Add(tarifa);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult CadastroTarifa()
        {
            return View();
        }

        public PartialViewResult ListaTarifas()
        {
            var model = new AdministradorViewModel();
            model.Tarifas = context.Tarifas.ToList();
            return PartialView("_listaTarifas", model.Tarifas);
        }
        
        public async Task<PartialViewResult> ListaVendas()
        {
            var model = new AdministradorViewModel();
            model.Vendas = await vendaServices.GetAllVendas();
            model.ObterLucro();

            return PartialView("_listaVendas", model.Vendas);
        }

        public async Task<PartialViewResult> AllVendas()
        {
            var model = new AdministradorViewModel();
            model.Vendas = await vendaServices.GetAllVendas();

            return PartialView("_allVendas", model.Vendas);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarifa = context.Tarifas.Find(id);

            if (tarifa == null)
            {
                return NotFound();
            }

            return View(tarifa);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Tarifa tarifa)
        {            
            if (id != tarifa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(tarifa);
                    context.SaveChanges();
                }
                catch 
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarifa = context.Tarifas.FirstOrDefault(t => t.Id == id);

            if (tarifa == null)
            {
                return NotFound();
            }

            return View(tarifa);
        }

        // POST: Admin/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var tarifa = context.Tarifas.Find(id);
            context.Tarifas.Remove(tarifa);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));        
        }

        // GET: Admin/DeleteVenda/5
        public async Task<ActionResult> DeleteVenda(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await vendaServices.GetVendaByIdAsync(id.Value);

            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // POST: Admin/DeleteVenda/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteVenda(int id)
        {
            var venda = vendaServices.GetVendaById(id);
            await vendaServices.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}