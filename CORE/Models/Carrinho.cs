using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Carrinho : ICarrinho
    {
        public List<ItemCarrinho> Itens { get; set; }
    }
}
