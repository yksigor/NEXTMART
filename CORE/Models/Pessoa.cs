using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Models
{
    [Table("PESSOAS")]
    public abstract class Pessoa
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Usuario))]
        public int IdUsuario { get; set; }

        public virtual Usuario Usuario { get; set; }

        [ForeignKey(nameof(Endereco))]
        public int IdEndereco { get; set; }

        public virtual Endereco Endereco { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Telefone { get; set; }

        [Display(Name = "Saldo Carteira")]
        public double Saldo { get; set; }

    }
}
