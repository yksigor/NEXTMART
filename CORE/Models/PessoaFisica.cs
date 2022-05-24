using Domain.Validate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Domain.Models
{
    [Table("PESSOASFISICAS")]
    [Display(Name = "Pessoa Física")]
    public class PessoaFisica : Pessoa
    {
        [CPF]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Display(Name = "Nome Completo")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O campo Sexo é obrigatório")]
        public Sexo? Sexo { get; set; }

        public virtual List<Sexo> GetSexos() => Enum.GetValues(typeof(Sexo)).Cast<Sexo>().ToList();

        [Display(Name = "Status")]
        public StatusConsumidor StatusConsumidor { get; set; }

        public virtual List<StatusConsumidor> GetStatusConsumidor => Enum.GetValues(typeof(StatusConsumidor)).Cast<StatusConsumidor>().ToList();
    }

    public enum Sexo
    {
        Feminino = 0,
        Masculino = 1,
    }
    public enum StatusConsumidor
    {
        [Display(Name = "Ativo")]
        Ativo = 0,
        [Display(Name = "Inativo")]
        Inativo = 1,
    }
}
