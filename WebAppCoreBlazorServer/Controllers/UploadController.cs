using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAppCoreBlazorServer.Controllers
{
    //dongpv:upload image
    //[Route("api/[controller]")]
    //[ApiController]
    [DisableRequestSizeLimit]
    public class UploadController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public UploadController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost("upload/single")]
        public IActionResult Single(IFormFile file)
        {
            try
            {
                UploadFile(file);

                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("upload/multiple")]
        public IActionResult Multiple(IFormFile[] files)
        {
            try
            {
                foreach (var file in files)
                {
                    UploadFile(file);
                }

                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("upload/{id}")]
        public IActionResult Post(IFormFile[] files, int id)
        {
            try
            {
                // Put your code here
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        public async Task UploadFile(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    var imagePath = @"\Upload";
                    var uploadPath = _environment.WebRootPath + imagePath;

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    var fullPath = Path.Combine(uploadPath, file.FileName);
                    using (FileStream fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
