using CrossNextMart.Views;
using Domain.Models;
using Domain.Security;
using SServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrossNextMart
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Pedidos : ContentPage
    {
        private readonly JWTServices service;
        private readonly Token token;
        public List<Venda> Vendas { get; private set; }

        public Pedidos(Token token, JWTServices service)
        {
            InitializeComponent();
            this.token = token;
            this.service = service;

            NavigationPage.SetHasBackButton(this, false);

            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#222");
            CarregarListView();

        
        }

        private async void CarregarListView()
        {
            Vendas = await service.GetPedidosAsync(token);

            var pe = new List<Pedido>();

            Vendas.ForEach(pedido =>
            {
                var p = new Pedido()
                {
                    Id = pedido.Id,
                    Comerciante = pedido.Itens.Count() > 0 ? pedido.Itens.First().Produto.Comerciante.NomeFantasia : "",
                    Total = pedido.GetValorTotal(),
                    Data = pedido.DataVenda.ToString("dd/MM/yyyy"),
                    Status = pedido.StatusVenda.ToString()
                };
                pe.Add(p);
            });

            listView.ItemsSource = pe;
        }

        private async void BtnSair_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var pedido = ((sender as ListView).SelectedItem as Pedido);

            if (pedido.Total == 0 || pedido.Comerciante == "")
            {
                await DisplayAlert("Alert!!", "Não exite item neste pedido" ,"Sair");
                return;
            }

            var pedidoCompleto = Vendas.FirstOrDefault(p => p.Id == pedido.Id);

            var detalhe = new DetalhePedido(pedidoCompleto)
            {
                Title = $"Detalhes Pedido {pedidoCompleto.Id}"
            };

            await Navigation.PushAsync(detalhe);


        }
    }
   

    public class Pedido
    {
        public int Id { get; set; }
        public string Comerciante { get; set; }
        public double Total { get; set; }
        public string Data { get; set; }
        public string Status { get; set; }
    }
}