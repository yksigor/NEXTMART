using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace WebMVC.Util
{
    public static class FileUtil
    {
        public static async Task<ImagemProduto> InserirImagem(IFormFile source, int idProduto, string wwwrootPath)
        {
            var nomeImage = EnsureCorrectFilename(ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"'));
            var dirUploads = "uploads";
            var imagePath = GetUploadsPathByEnvironment(wwwrootPath, dirUploads);
            var _imgName = $"P{idProduto}_{nomeImage}";

            var imagem = new ImagemProduto
            {
                IdProduto = idProduto,
                NomeImagem = _imgName,
                CaminhoImagem = $"{imagePath}{_imgName}",
                UrlImagem = $"/{dirUploads}/{_imgName}"
            };

            using (var stream = new FileStream(imagem.CaminhoImagem, FileMode.Create))
            {
                await source.CopyToAsync(stream);
            }

            return imagem;
        }

        private static string GetUploadsPathByEnvironment(string wwwrootPath, string dirUploads) =>
            wwwrootPath.Contains(":") ? $"{wwwrootPath}\\{dirUploads}\\" : $"{wwwrootPath}/{dirUploads}/";

        public static bool ImagemExiste(string wwwrootPath, string nomeImagem)
        {
            var file = GetUploadsPathByEnvironment(wwwrootPath, "uploads") + nomeImagem;
            return File.Exists(file);
        }

        private static string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }
    }
}
