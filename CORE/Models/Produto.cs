using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Globalization;

namespace Domain.Models
{
    [Table("PRODUTOS")]
    public class Produto
    {

        CultureInfo ptBR = new CultureInfo("pt-BR");

        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(PessoaJuridica))]
        [Display(Name = "Loja")]
        public int IdComerciante { get; set; }

        public virtual PessoaJuridica Comerciante { get; set; }

        [Required]
        [ForeignKey(nameof(CategoriaProduto))]
        [Display(Name = "Categoria")]
        public int IdCategoria { get; set; }

        [Display(Name = "Categoria")]
        public virtual CategoriaProduto Categoria { get; set; }
        [Required, Display(Name = "Nome")]
        public string Nome { get; set; }
        [Required, Display(Name = "Marca")]
        public string Marca { get; set; }
        [Required, Display(Name = "Descrição")]
        public string Descricao { get; set; }
        [Required, Display(Name = "Preço")]
        public double Preco { get; set; }
        [Required, Display(Name = "Quantidade em Estoque")]
        public double QuantidadeEstoque { get; set; }

        [Display(Name = "Imagens do Produto")]
        public virtual List<ImagemProduto> ImagensProduto { get; set; }
    }
}