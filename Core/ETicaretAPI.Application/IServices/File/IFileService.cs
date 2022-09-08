using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.IServices.File
{
   public interface IFileService
    {
        //geriye bir list dönücekfile name ve path alan 
       Task<List<(string fileName,string path)>> UploadAsync(string path,IFormFileCollection files); //hangi path kaydettigini bildrien geri dönüş deperi ayarlandı //birden fazla dosya oldugu için birden fazla geri donus degeri olarak tasarlandı
       Task<string> FileRenameAsync(string fileName); //string döndürür
       Task<bool> CopyFileAsync(string path,IFormFile file); //sonuc true false döndürür
    }
}
