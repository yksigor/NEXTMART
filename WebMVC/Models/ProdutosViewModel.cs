using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace WebMVC.Models
{
    public class ProdutosViewModel
    {
        public List<CategoriaProduto> DropDownCategorias { get; set; }
        public List<PessoaJuridica> Lojas { get; set; }
        public Produto Produto { get; set; }

        public ProdutosViewModel()
        {

        }

        public ProdutosViewModel(List<CategoriaProduto> DropDownCategorias)
        {

        }
    }
}
