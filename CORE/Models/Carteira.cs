using Domain.Validate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Globalization;


namespace Domain.Models
{

    [Table("CARTEIRA")]
    public class Carteira
    {
        CultureInfo ptBR = new CultureInfo("pt-BR");

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "É obrigatório um valor")]
        [Display(Name = "Crédito")]
        public double Credito { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public TipoCredito TipoCredito { get; set; }

        [Required]
        [ForeignKey(nameof(PessoaFisica))]
        public int IdPessoaFisica { get; set; }

        public virtual PessoaFisica PessoaFisica { get; set; }
        
        public virtual List<TipoCredito> GetTipoCreditos => Enum.GetValues(typeof(TipoCredito)).Cast<TipoCredito>().ToList();
    }
    public enum TipoCredito
    {
        Débito,
        VR,
        VA,
        Dinheiro
    }

    public class Pagamento
    {
        public int IdComerciante { get; set; }
        public double Valor { get; set; }
    }
}
