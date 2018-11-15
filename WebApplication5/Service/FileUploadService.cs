using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebApplication5.Service
{
    public class FileUploadService
    {
        public async void Upload(string path, string fileName, IFormFile file)
        {
            Directory.CreateDirectory(path);
            using (var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
    }
}
