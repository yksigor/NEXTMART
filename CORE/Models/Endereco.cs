using Domain.Validate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Domain.Models
{
    [Table("ENDERECOS")]
    public class Endereco
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Logradouro { get; set; }

        [Display(Name ="Número")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Numero { get; set; }

        public string Complemento { get; set; }

        [CEP(ErrorMessage = "O formato de CEP não é válido")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string CEP { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public UF? UF { get; set; }

        [Display(Name ="Município")]
        public string Municipio { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Bairro { get; set; }

        public List<UF> GetUFs() => Enum.GetValues(typeof(UF)).Cast<UF>().ToList();
    }

    public enum UF
    {
        AC,
        AL,
        AM,
        AP,
        BA,
        CE,
        DF,
        ES,
        GO,
        MA,
        MG,
        MS,
        MT,
        PA,
        PB,
        PE,
        PI,
        PR,
        RJ,
        RN,
        RO,
        RR,
        RS,
        SC,
        SE,
        SP,
        TO
    }

}
