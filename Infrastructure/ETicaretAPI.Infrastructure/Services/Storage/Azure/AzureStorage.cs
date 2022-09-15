using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ETicaretAPI.Application.Abstractions.Storage.Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Storage.Azure
{
    public class AzureStorage : Storage, IAzureStorage
    {
       private readonly BlobServiceClient _blobServiceClient; //azure baglanmayı saglar
       BlobContainerClient _blobContainerClient; //o clienttaki container üzerinde dosya işlemleri yapılmasını saglar
        public AzureStorage(IConfiguration configuration)
        {
            _blobServiceClient = new(configuration["Storage:Azure"]); //hangi store account baglancak(appsettings key veriyo)
        }
        public async Task DeleteAsync(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }

        public List<string> GetFiles(string containerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Select(b => b.Name).ToList();
        }

        public bool HasFile(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Any(b => b.Name == fileName);
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string containerName, IFormFileCollection files)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName); //hangi container ile calısılcak bildirip nesne olusturuldu
            await _blobContainerClient.CreateIfNotExistsAsync(); //varmı yokmu yoksa oluştur
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer); //

            List<(string fileName, string pathOrContainerName)> datas = new(); //topple olusturuldu
            foreach (IFormFile file in files) 
            {
                string fileNewName = await FileRenameAsync(containerName, file.Name, HasFile);

                BlobClient blobClient = _blobContainerClient.GetBlobClient(fileNewName); //dosya ismini veriyoruz
                await blobClient.UploadAsync(file.OpenReadStream()); //stream olrak azure gönderildi
                datas.Add((fileNewName, $"{containerName}/{fileNewName}"));
            }
            return datas;
        }
    }
}
