using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    [Table("CATEGORIAPRODUTOS")]
    public class CategoriaProduto
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(PessoaJuridica))]
        public int IdComerciante { get; set; }

        public virtual PessoaJuridica Loja { get; set; }

        [Required]
        public string Nome { get; set; }

        [Display(Name ="Descrição")]
        public string Descricao { get; set; }

    }
}
