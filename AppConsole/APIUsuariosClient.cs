using Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace AppConsole
{
    public class APIUsuariosClient
    {
        private readonly HttpClient _client;

        public APIUsuariosClient(HttpClient client)
        {
            _client = client;
        }

        public List<Usuario> ListarUsuarios()
        {
            HttpResponseMessage response = _client.GetAsync(
                "usuarios").Result;

            List<Usuario> resultado = null;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string conteudo = response.Content.ReadAsStringAsync().Result;
                resultado = JsonConvert.DeserializeObject<List<Usuario>>(conteudo);
            }
            else
                Console.WriteLine("Token provavelmente expirado!");

            return resultado;
        }
    }
}
