using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Domain.Models;
using System.Threading.Tasks;
using SServices;
using Domain.Security;
using Newtonsoft.Json;
using System.IO;
using CrossNextMart.Banco;
using System.Text;

namespace CrossNextMart
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
        private readonly JWTServices service;
        private Token token;
        private readonly Repositorio repositorio;
        private string URL => Utils.Constants.UrlBaseAPI_AWS;

        public Login()
		{                       
            InitializeComponent();
         
            service = new JWTServices(URL);
            repositorio = new Repositorio();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var usuario = PegarUsuarioDigitado();
            if(usuario != null)
                await EfetuarLogin(usuario);
        }

        private async Task EfetuarLogin(IUsuario usuario)
        {
            if (await GetToken(usuario))
            {
                await repositorio.GravarAsync(token);
                await EntrarPerfil();
            } else
                await DisplayAlert("Erro", "Não foi possível autenticar", "OK");
        }

        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            return await service.GetUsuariosAsync(token);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            LimparCampos();

            if (URL.Contains("https")) return;
            if (await ValidarURL())
                ConferirDbToken();
            else
                this.IsVisible = false;
        }

        private async Task EntrarPerfil()
        {
            if (token.AccessToken is null) return;

            var tipoUser = await service.GetTipoUserAsync(token);

            if (tipoUser.HasValue && tipoUser.Value >= 0)
                if (tipoUser.Value == 0)
                    await Navigation.PushAsync(new Perfil(token, service, repositorio));
                else
                {
                    await DisplayAlert("Não permitido", "Efetue login com um usuário do tipo CONSUMIDOR", "OK");
                    LimparCampos();
                    await repositorio.RemoverAsync();
                }
        }

        private async void ConferirDbToken()
        {
            token = repositorio.ObterTokenDoArquivo();

            if (token != null)
            {
                await EntrarPerfil();
            }
        }

        private async Task<bool> ValidarURL()
        {
            var urlOK = await service.UrlIsValid();

            var msg = URL.Contains("https") ? "O aplicativo ainda não funciona com API https, mude para http"
                : !urlOK ? $"Erro Fatal: Não foi possível chegar em {URL}"
                : URL.Contains("localhost") ? "Certifique que seu dispositivo tem acesso ao localhost!" : null;

            if (msg != null)
            {
                await DisplayAlert("Atenção", msg, "OK");
            }

            return (!URL.Contains("https") && urlOK);
        }

        private IUsuario PegarUsuarioDigitado()
        {
            var user = Username.Text;
            var senha = Password.Text;

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(senha))
            {
                DisplayAlert("Erro", "Verifique os campos", "OK");
                return null;
            }

            return new User{ Username = user, Password = senha };
        }

        private async Task<bool> GetToken(IUsuario usuario)
        {
            token = await service.GetTokenOnlyHttp(usuario.Username, usuario.Password);
            return token.Message == "OK";
        }

        private void LimparCampos()
        {
            Username.Text = Password.Text = "";
        }
    }
}