using ETicaretAPI.Application.Abstractions.Storage.Local;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Storage.Local
{
    public class LocalStorage :Storage, ILocalStorage
    {

        private readonly IWebHostEnvironment _webHostEnvironment;

        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task DeleteAsync(string pathOrContainerName, string fileName)
        {
            System.IO.File.Delete($"{pathOrContainerName}\\{fileName}");
        }

        public List<string> GetFiles(string pathOrContainerName)
        {
            DirectoryInfo directory = new(pathOrContainerName);
           return directory.GetFiles().Select(f => f.Name).ToList();
        }

        public  bool HasFile(string pathOrContainerName, string fileName)
        {
            System.IO.File.Exists($"{pathOrContainerName}\\{fileName}");
            return true;
            
        }

        private async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync(); //yapılan calısmları temizle
                return true;
            }
            catch (Exception ex)
            {
                //log!
                throw ex;
            }
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
        {
        
                //wwwroot/resource/product-images
                string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, pathOrContainerName);

                if (!Directory.Exists(uploadPath)) //yoksa oluştur
                {
                    Directory.CreateDirectory(uploadPath);
                }
                List<(string fileName, string path)> datas = new();
                //koleksiyon oluşturma
                List<bool> results = new();

                //dosyaları yakalamak için
                foreach (IFormFile file in files)
                {
                    //string fileNewName = await FileRenameAsync(pathOrContainerName, file.Name, HasFile);
                    bool result = await CopyFileAsync($"{uploadPath}\\{file.Name}", file);
                    datas.Add((file.Name, $"{pathOrContainerName}\\{file.Name}"));
                    results.Add(result);
                }
                return datas;
           
            
           
           
        }
    }
}
