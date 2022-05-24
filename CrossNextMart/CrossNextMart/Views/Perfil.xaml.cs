using CrossNextMart.Banco;
using Domain.Security;
using SServices;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrossNextMart
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Perfil : ContentPage
	{
        private readonly Token token;
        private readonly JWTServices service;
        private readonly Repositorio repositorio;
        

        public Perfil (Token token, JWTServices service, Repositorio repositorio)
		{
			InitializeComponent();
            
            this.token = token;
            this.service = service;
            this.repositorio = repositorio;

            NavigationPage.SetHasBackButton(this, false);

            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#222");
        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var pessoa = await service.GetPessoaFisicaAsync(token);
            DateTime idade = pessoa.DataNascimento;
            TimeSpan ts = DateTime.Today - idade;
            DateTime idt = (new DateTime() + ts).AddYears(-1).AddDays(-1);
            Nome.Text = pessoa.NomeCompleto;
            Idade.Text = Convert.ToString(idt.Year);
            Sexo.Text = Convert.ToString(pessoa.Sexo);
            Endereco.Text = pessoa.Endereco.Logradouro;
            Municipio.Text = pessoa.Endereco.Municipio;
            Bairro.Text = pessoa.Endereco.Bairro;
            Numero.Text = pessoa.Endereco.Numero;
            Uf.Text = Convert.ToString(pessoa.Endereco.UF);
        }

        private async void BtnSair_Clicked(object sender, EventArgs e)
        {
            if (await ConfirmarSair())
            {
                await repositorio.RemoverAsync();
                await Navigation.PopAsync();
            }
        }

        private async Task<bool> ConfirmarSair()
        {
            var resp = await DisplayAlert("Logout", "Tem certeza que deseja sair?", "Sim", "Cancelar");
            return resp;
        }

        protected override bool OnBackButtonPressed()
        {
            BtnSair_Clicked(null, null);
            return true;
        }

        private async void BtnPedidos(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pedidos(token, service));
        }
    }
}