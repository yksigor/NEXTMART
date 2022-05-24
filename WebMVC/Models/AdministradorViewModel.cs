using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Models
{
    public class AdministradorViewModel
    {
        public Usuario Usuario { get; set; }
        public List<Tarifa> Tarifas { get; set; }
        public List<Venda> Vendas { get; set; }

        public void ObterLucro()
        {
            Vendas.ForEach(venda =>
            {
                Console.WriteLine(venda.GetLucroSistema());
            });
        }
    }
}
