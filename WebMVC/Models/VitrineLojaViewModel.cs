using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Models
{
    public class VitrineLojaViewModel
    {
        public List<Produto> Produtos { get; set; }
        public Usuario User { get; set; }
        public Produto Produto {get; set;}
        public List<CategoriaProduto> CategoriaProdutos { get; set; }
    }
}
