using CrossNextMart.Utils;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrossNextMart.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetalhePedido : ContentPage
    {
        private readonly Venda Pedido;

        public DetalhePedido(Venda pedido)
        {
            this.Pedido = pedido;
			InitializeComponent();
            ListView.ItemsSource = GetListItemDetalhe(pedido.Itens);
  
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private List<ItemDetalhe> GetListItemDetalhe(List<Item> items)
        {
            var itensDetalhes = new List<ItemDetalhe>();

            items.ForEach(item =>
            {
                itensDetalhes.Add(new ItemDetalhe(item.Produto, item.Quantidade, item.ValorUnidade));
            });

            return itensDetalhes;
        }
    }

    public class ItemDetalhe : Item
    {
        public double GetValorTotal => base.ValorTotal();
        public string GetUrlImage { get; private set; }
        public string LblValorTotal { get; set; }
        public string LblValorUnid { get; set; }


        public ItemDetalhe(Produto produto, int quantidade, double valorUnidade)
        {
            base.Produto = produto;
            base.Quantidade = quantidade;
            base.ValorUnidade = valorUnidade;
            this.GetUrlImage = UrlImagem(produto.ImagensProduto.First().UrlImagem);
            LblValorTotal = base.StrValorTotal();
            LblValorUnid = base.StrValorUnid();
        }

        private string UrlImagem(string urlRelativa)
        {
            return string.Concat(Constants.ServidorImg,urlRelativa);
        }


    }
}
