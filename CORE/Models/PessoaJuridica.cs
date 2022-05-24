using Domain.Validate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Domain.Models
{
    [Table("PESSOASJURIDICAS")]
    public class PessoaJuridica : Pessoa
    {
        [CNPJ]
        public string Cnpj { get; set; }

        [Display(Name = "Razão Social")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string RazaoSocial { get; set; }

        [Display(Name = "Nome Fantasia")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string NomeFantasia { get; set; }

        [Display(Name = "Segmento")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Segmento { get; set; }

        [Display(Name = "Status")]
        public StatusComerciante StatusComerciante { get; set; }

        public virtual List<StatusComerciante> GetStatusComerciante => Enum.GetValues(typeof(StatusComerciante)).Cast<StatusComerciante>().ToList();
    }
    public enum StatusComerciante
    {
        [Display(Name = "Ativo")]
        Ativo = 0,
        [Display(Name = "Inativo")]
        Inativo = 1,
    }
}
