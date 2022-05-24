using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebMVC.Controllers
{
    [Route("api/files")]
    public class FilesController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public FilesController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Index(IFormCollection formCollection)
        {
            var imagens = new List<String>();

            foreach (var source in formCollection.Files)
            {
                string filename = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');

                filename = this.EnsureCorrectFilename(filename);
                var fullFilePath = this.GetPathAndFilename(filename);

                using (var stream = new FileStream(fullFilePath, FileMode.Create))
                {
                    await source.CopyToAsync(stream);
                }

                imagens.Add(string.Concat("uploads/",filename));
            }

            return Ok(imagens);
        }

        private string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        private string GetPathAndFilename(string filename)
        {
            return hostingEnvironment.WebRootPath + "\\uploads\\" + filename;
        }
    }
}