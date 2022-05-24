using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

// encontrar uma forma de criar unique key composta com dataanottation para os campos [id,produto]
namespace Domain.Models
{
    [Table("ITENS")]
    public class Item
    {
        CultureInfo ptBR = new CultureInfo("pt-BR");

        public int Id { get; set; }
        public int Quantidade { get; set; }

        [Display(Name = "Valor Unidade")]
        public double ValorUnidade { get; set; }



        [ForeignKey(nameof(Venda))]
        [Required]
        [ConcurrencyCheck]
        public int VendaId { get; set; }
        //public virtual Venda Venda { get; set; }

        [ForeignKey(nameof(Produto))]
        [Required]
        [ConcurrencyCheck]
        public int ProdutoId { get; set; }
        public virtual Produto Produto { get; set; }

        public double ValorTotal() => Quantidade * ValorUnidade;

        public string StrValorTotal() => ValorTotal().ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"));

        public string StrValorUnid() => ValorUnidade.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR"));
    }
}
