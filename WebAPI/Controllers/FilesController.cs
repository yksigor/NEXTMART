using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly string _path;

        public FilesController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;

            if (hostingEnvironment.IsProduction())
            {
                _path =  Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                _path = Path.Combine(_path,"app/uploads/");
            }
            else
            {
                _path = Directory.GetCurrentDirectory();
            }

        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            return new JsonResult(await WriteFile(file));
        }

        [HttpDelete("{fileName}")]
        public IActionResult RemoveImage(string fileName)
        {
            var fi = new FileInfo(Path.Combine(_path, fileName));
            try
            {
                fi.Delete();

                return Ok();
            }
            catch (IOException e)
            {
                return new JsonResult(e.Message);
            }
        }

        private async Task<string> WriteFile(IFormFile file)
        {
            string fileName;
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                fileName = Guid.NewGuid().ToString() + extension; //Create a new Name 
                                                                  //for the file due to security reasons.
                var path = Path.Combine(_path, fileName);

                using (var bits = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(bits);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return fileName;
        }
    }
}
