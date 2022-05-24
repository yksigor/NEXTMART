using Domain.Models;
using Domain.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SServices
{
    public class JWTServices
    {
        private readonly string urlBase;

        public JWTServices(string urlBase)
        {
            this.urlBase = urlBase;
        }

        public async Task<Token> RequestTokenAsync(string login, string senha)
        {
            using (var client = Client())
            {
                HttpResponseMessage respToken = client.PostAsync("login", new StringContent(
                    JsonConvert.SerializeObject(new
                    {
                        Username = login,
                        Password = senha
                    }), Encoding.UTF8, "application/json")).Result;

                var conteudo = await respToken.Content.ReadAsStringAsync();

                if (respToken.StatusCode.Equals(HttpStatusCode.OK))
                {
                    return JsonConvert.DeserializeObject<Token>(conteudo);
                }
                return null;
            }
        }

        public async Task<Token> RequestTokenMobileAsync(HttpClient client, string login, string senha)
        {
            HttpResponseMessage respToken = client.PostAsync("login", new StringContent(
                JsonConvert.SerializeObject(new
                {
                    Username = login,
                    Password = senha
                }), Encoding.UTF8, "application/json")).Result;

            var conteudo = await respToken.Content.ReadAsStringAsync();

            if (respToken.StatusCode.Equals(HttpStatusCode.OK))
            {
                return JsonConvert.DeserializeObject<Token>(conteudo);
            }
            return null;
        }

        public async Task<Token> GetTokenOnlyHttp(string login, string senha)
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri(string.Concat(urlBase, "login"));

                var user = new User { Username = login, Password = senha };

                var json = JsonConvert.SerializeObject(user);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage respToken = await client.PostAsync(uri, content);

                var conteudo = await respToken.Content.ReadAsStringAsync();


                Token token = null;
                if (respToken.StatusCode.Equals(HttpStatusCode.OK))
                {
                    token = JsonConvert.DeserializeObject<Token>(conteudo);
                }

                return token;
            }
        }

        public async Task<List<Usuario>> GetUsuariosAsync(Token token)
        {
            using(var client = Client(token))
            {
                HttpResponseMessage response = await client.GetAsync("usuarios");

                var resultado = new List<Usuario>();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string conteudo = await response.Content.ReadAsStringAsync();
                    resultado = JsonConvert.DeserializeObject<List<Usuario>>(conteudo);
                }
                return resultado;
            }
        }

        public async Task<int?> GetTipoUserAsync(Token token)
        {
            using (var client = Client(token))
            {
                HttpResponseMessage response = await client.GetAsync("login");

                int resultado = -1;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string conteudo = await response.Content.ReadAsStringAsync();
                    resultado = JsonConvert.DeserializeObject<int>(conteudo);
                }
                return resultado;
            }
        }

        public async Task<List<Venda>> GetPedidosAsync(Token token)
        {
            using(var client = Client(token))
            {
                HttpResponseMessage response = await client.GetAsync("pedidos");

                var resultado = new List<Venda>();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string conteudo = await response.Content.ReadAsStringAsync();
                    resultado = JsonConvert.DeserializeObject<List<Venda>>(conteudo);
                }
                return resultado;
            }
        }


        public async Task<PessoaFisica> GetPessoaFisicaAsync(Token token)
        {
            using (var client = Client(token))
            {
                HttpResponseMessage response = await client.GetAsync("pessoafisicas");

                var resultado = new PessoaFisica();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string conteudo = await response.Content.ReadAsStringAsync();
                    resultado = JsonConvert.DeserializeObject<PessoaFisica>(conteudo);
                }

                return resultado;
            }
        }


        private HttpClient Client()
        {
            var httpClientHandler = new HttpClientHandler();
            //httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            var client = new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri(urlBase)
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private HttpClient Client(Token token)
        {
            var client = Client();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            return client;
        }

        public async Task<bool> UrlIsValid()
        {
            try
            {
                using(var client = Client())
                {
                    client.Timeout = TimeSpan.FromMilliseconds(2000);
                    HttpResponseMessage response = await client.GetAsync("test");

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
}
