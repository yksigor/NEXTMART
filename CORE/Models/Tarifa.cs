using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace Domain.Models
{
    [Table("TARIFAS")]
    public class Tarifa
    {
        CultureInfo ptBR = new CultureInfo("pt-BR");

        [Key]
        public int Id { get; set; }

        [Required]
        public string Codigo { get; set; }
        [Required]
        public double Porcentagem { get; set; }
        [Required]
        public double ValorMaximo { get; set; }
    }
}
