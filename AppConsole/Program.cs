using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using SServices;

namespace AppConsole
{
    class Program
    {
        private static string _urlBase;

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile($"appsettings.json");
            var config = builder.Build();

            _urlBase = config.GetSection("AcessoWebAPI:UrlBase").Value;

            var service = new JWTServices(_urlBase);

            var token = service.RequestTokenAsync(config.GetSection("AcessoWebAPI:Username").Value, config.GetSection("AcessoWebAPI:Password").Value).Result;

            if (token.Authenticated)
            {
                var usuarios = service.GetUsuariosAsync(token).Result;

                usuarios.ForEach(l =>
                {
                    Console.WriteLine(l.Username);
                });
            }

            Console.ReadKey();
        }
    }
}
