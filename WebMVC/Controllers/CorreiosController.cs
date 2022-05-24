using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Domain.Validate;
using Domain.ViaCEP;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebMVC.Controllers
{
    public class CorreiosController : Controller
    {
        /// <summary>
        /// The base URL
        /// </summary>
        private const String BaseUrl = "https://viacep.com.br";


        public async Task<JsonResult> BuscarCEP(string zipCode)
        {
            if (Validacao.Cep(zipCode))
            {
                //try
                //{
                //    return Json(SearchAsync(zipCode, CancellationToken.None, null).Result);
                //}
                //catch
                //{
                //    return Json(SearchAsync(zipCode, CancellationToken.None, GetHandler()).Result);
                //}

                var cep = await PegarCEP(zipCode);

                return new JsonResult(cep);
            }

            return new JsonResult(false);
        }


        private HttpClientHandler GetHandler()
        {
            var proxyUrl = @"http://smapro-cl1:9090";
            var proxyUsername = @"alanbarros";
            var proxyPassword = @"Winterwolf3647";
            var proxyDomain = @"sma.local";
            var authType = "NTLM";

            var credCache = new CredentialCache();
            var credentials = new NetworkCredential(proxyUsername, proxyPassword, proxyDomain);
            credCache.Add(new Uri(proxyUrl), authType, credentials);

            return new HttpClientHandler
            {
                UseProxy = true,
                Proxy = new WebProxy
                {
                    Address = new Uri(proxyUrl),
                    Credentials = credCache
                }
            };
        }

        /*private async Task<ViaCEPResult> SearchAsync(String zipCode, CancellationToken token, HttpClientHandler handler)
        {
            using (var client = handler is null ? new HttpClient() : new HttpClient(handler))
            {
                client.BaseAddress = new Uri(BaseUrl);

                var resp2 = await client.GetAsync($"/ws/{zipCode}/json/");
                var response = await client.GetAsync($"/ws/{zipCode}/json/", token).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var viacep = response.Content.ReadAsAsync<ViaCEPResult>(token).ConfigureAwait(false);
                return await viacep;
            }
        }*/

        private async Task<ResultViaCEP> PegarCEP(string cep)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                var response = await client.GetAsync($"/ws/{cep}/json/");

                var resultado = new ResultViaCEP();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string conteudo = await response.Content.ReadAsStringAsync();
                    resultado = JsonConvert.DeserializeObject<ResultViaCEP>(conteudo);
                }
                return resultado;
            }
        }
    }

    public class ResultViaCEP
    {
        public string cep { get; set; }
        public string logradouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string localidade { get; set; }
        public string uf { get; set; }
        public string unidade { get; set; }
        public string ibge { get; set; }
        public string gia { get; set; }
    }
}