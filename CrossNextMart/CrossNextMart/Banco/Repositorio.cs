using Domain.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CrossNextMart.Banco
{
    public class Repositorio
    {
        private readonly string bancoJson = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "banco.json");

        public Repositorio()
        {
            if (!File.Exists(bancoJson)) GravarAsync(new Token()).Wait();
        }

        public async Task GravarAsync(Token token)
        {
            using (var writer = new StreamWriter(bancoJson))
            {
                var stringJson = JsonConvert.SerializeObject(token);
                await writer.WriteAsync(stringJson);
            }
        }

        public Token ObterTokenDoArquivo()
        {
            using (var reader = new StreamReader(bancoJson))
            {
                var texto = reader.ReadLine();

                var json = JsonConvert.DeserializeObject<Token>(texto);

                return json;
            }
        }

        public async Task RemoverAsync()
        {
            await GravarAsync(new Token());
        }

    }
}
