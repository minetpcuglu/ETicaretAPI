using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions
{
    //hepsinde ortak olan bir generic yapı kuruyoruz mesela azur local amazon vs...
    public interface IStorage
    {
        //geriye bir list dönücekfile name ve path alan 
        Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files); //hangi path kaydettigini bildrien geri dönüş deperi ayarlandı //birden fazla dosya oldugu için birden fazla geri donus degeri olarak tasarlandı
        Task DeleteAsync(string pathOrContainerName, string fileName);
        List<string> GetFiles(string pathOrContainerName);
        bool HasFile(string pathOrContainerName, string fileName); //varmı
    }
}
