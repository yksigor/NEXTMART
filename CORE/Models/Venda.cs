using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;

namespace Domain.Models
{
    [Table("VENDAS")]
    public class Venda
    {
        CultureInfo ptBR = new CultureInfo("pt-BR");

        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Usuario))]
        public int IdUsuario { get; set; }

        public virtual PessoaFisica Consumidor { get; set; }

        public virtual Usuario Usuario { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data da Compra")]
        public DateTime DataVenda { get; set; }

        public virtual List<Item> Itens { get; set; }

        [Display(Name = "Status")]
        public StatusVenda StatusVenda { get; set; }

        public virtual List<StatusVenda> GetStatusVendas => Enum.GetValues(typeof(StatusVenda)).Cast<StatusVenda>().ToList();

        public double GetValorTotal()
        {
            double valor = 0;

            Itens.ForEach(a =>
            {
                valor += a.ValorUnidade * a.Quantidade;
            });
            return valor;
        }
        
        private double GetValorCobranca(Tarifa tarifa)
        {            
            double cobranca;

            if (GetValorTotal() < tarifa.ValorMaximo)
            {
                cobranca = GetValorTotal() * tarifa.Porcentagem;

            } else
            {
                cobranca = tarifa.ValorMaximo * tarifa.Porcentagem;
            }

            return cobranca;
        }

        public double GetLucroSistema() => GetValorCobranca(new Tarifa { Porcentagem = 0.05, ValorMaximo = 1000.00 });

        public List<Pagamento> GetListaPagamentos()
        {
            var pagamentos = new List<Pagamento>();

            this.Itens.ForEach(item =>
            {
                var p = pagamentos.FirstOrDefault(a => a.IdComerciante == item.Produto.Id);

                if (p != null)
                {
                    p.Valor += item.ValorTotal();
                }
                else
                {
                    pagamentos.Add(new Pagamento
                    {
                        IdComerciante = item.Produto.IdComerciante,
                        Valor = item.ValorTotal()
                    });
                }
            });

            return pagamentos;
        }
    }

    public enum StatusVenda
    {
        [Display(Name = "Pendente de Pagamento")]
        Aberta = 0,
        [Display(Name = "Concluída")]
        Concluida = 1,
        [Display(Name = "Compra Cancelada")]
        Cancelada = 2
    }
}