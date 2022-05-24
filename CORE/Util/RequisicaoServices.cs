using Domain.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Util
{
    public static class RequisicaoServices
    {
        public static async Task<HttpResponseMessage> Post(string json, string rota)
        {
            var requisicao = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return await requisicao.PostAsync($"http://localhost:5000/api/{rota}", content);
        }

        public static async Task<HttpResponseMessage> Get(string controller)
        {
            var requisicao = new HttpClient();
            return await requisicao.GetAsync($"http://localhost:5000/api/{controller}");
        }

        public static async Task<HttpResponseMessage> Get(string json, string controller)
        {
            var requisicao = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var conteudo = new HttpRequestMessage { Content = content, RequestUri = new System.Uri(controller) };
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return await requisicao.SendAsync(conteudo);
        }
    }
}
