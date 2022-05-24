using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Domain.Models
{
    public class ItemCarrinho
    {
        CultureInfo ptBR = new CultureInfo("pt-BR");


        public int IdProduto { get; set; }
        public int Quantidade { get; set; }
        public double Valor { get; set; }
    }
}
