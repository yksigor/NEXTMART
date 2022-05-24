using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Data;
using Services.Contratos;

namespace WebMVC.Controllers
{
    [Authorize]
    public class LojasController : Controller
    {
        private readonly ILoginServices _services;
        private readonly LojaDBContext _context;

        public LojasController(LojaDBContext context, ILoginServices services)
        {
            _context = context;
            _services = services;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public JsonResult GetEnderecos(string idLojas)
        {
            var listaEnderecos = new List<string>();
            
            var endereco = _context.Enderecos.Find(Convert.ToInt32(idLojas));
            var enderecoString = endereco.Logradouro + " " + endereco.Numero + ", " + endereco.CEP;
            listaEnderecos.Add(enderecoString);

            return Json(listaEnderecos);
        }

        public JsonResult GetLojas()
        {
            var listaString = new List<string>();

            var listaLojas = _context.PessoasJuridicas.ToList();

            foreach (var l in listaLojas)
            {
                var idLoja = l.Id.ToString() ;
                listaString.Add(idLoja);
            }

            return Json(listaString);
        }

        public JsonResult GetInfo(string idLojas)
        {
            var pessoaJuridica = _context.PessoasJuridicas.Find(Convert.ToInt32(idLojas));

            var enderecoLoja = _context.Enderecos.FirstOrDefault(a => a.Id == pessoaJuridica.IdEndereco);

            pessoaJuridica.Endereco = enderecoLoja;

            return Json(pessoaJuridica);
        }

        public JsonResult GetProdutos(string idLojas)
        {
            var produtos = new List<CategoriaProduto>();

            var pessoaJuridica = _context.PessoasJuridicas.Find(Convert.ToInt32(idLojas));

            if (pessoaJuridica is null)
            {
                return null;
            }

            produtos = _context.CategoriaProdutos.Where(a => a.IdComerciante == pessoaJuridica.Id).Take(5).ToList();

            return Json(produtos);
        }
    }
}