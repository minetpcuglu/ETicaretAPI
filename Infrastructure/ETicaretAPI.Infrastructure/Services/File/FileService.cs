using ETicaretAPI.Application.IServices.File;
using ETicaretAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ETicaretAPI.Infrastructure.Services.File
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> CopyFileAsync(string path, IFormFile file)
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

        async Task<string> FileRenameAsync(string path, string fileName, bool first = true) //dısarıdan ihtiyac varmı diye sormaya gerek yok (private)
        {
            //dosyaların adını düzenleme
            // 1- dosyanın adını olmaması gereken karakterlerden düzenleme
            // 2- dosya adına ilgili dizinde varmı yokmu kontrol varsa ilgili dizinin sonuna-2 -3  vs eklenmesi
            string newFileName = await Task.Run<string>(async () =>
            {
                //Task Run asycn fonk uzerınden gerceklştirmek icin eklendi
                string extension = Path.GetExtension(fileName);
                string newFileName = string.Empty;
                if (first)
                {
                    string oldName = Path.GetFileNameWithoutExtension(fileName);
                    newFileName = $"{NameOperation.CharacterRegulatory(oldName)}{extension}"; //karakterleri inglizce klavyeye göre ayarlama
                }
                else 
                {
                    newFileName = fileName;
                    int indexNo1 = newFileName.IndexOf("-");
                    if (indexNo1 == -1)
                    {
                        newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                    }
                    else //dosya yolunda "-" varsa
                    {
                        int lastIndex = 0;
                        while (true)//indexten sonraki ilk "-" 
                        {
                           lastIndex = indexNo1;
                           indexNo1 = newFileName.IndexOf("-", indexNo1 + 1);
                            if (indexNo1==-1)
                            {
                                indexNo1 = lastIndex;
                                break;
                            }
                        }

                        int indexNo2 = newFileName.IndexOf(".");
                        //substring() => kullanıldığı string tipli değişkende içeriğin belli bir kısmının alınmasını 
                        // "." ile en son bulunan "-" arasındaki degeri alıyoruz
                        string fileNo = newFileName.Substring(indexNo1+1, indexNo2 - indexNo1-1); //"Mine123.png"

                        if (int.TryParse(fileNo, out int _fileNo))
                        {
                            _fileNo++;
                            //aynı isimde olan dosyların sonuna -1,2,3 eklendi
                            newFileName = newFileName.Remove(indexNo1+1, indexNo2 - indexNo1 - 1).Insert(indexNo1+1, _fileNo.ToString());
                        }
                        else
                            newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                    }
                }
                if (System.IO.File.Exists($"{path}\\{newFileName}")) //dosya varmı kontrolu varsa "-" koy 
                    return await FileRenameAsync(path, newFileName, false);
                else
                    return newFileName;
            });
            return newFileName;
        }

        public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
        {
            Random r = new();
            //wwwroot/resource/product-images
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);

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
                string fileNewName = await FileRenameAsync(uploadPath, file.FileName);
                bool result = await CopyFileAsync($"{uploadPath}\\{fileNewName}", file);
                datas.Add((fileNewName, $"{uploadPath}\\{fileNewName}"));
                results.Add(result);
            }
            if (results.TrueForAll(r => r.Equals(true))) //resultlarin hepsi true mu  degilse true eşitle
            {
                return datas;
            }
            else
            {
                //hata fırlat
                //yukarıdaki if geçerli değil ise burada dosyaların sunucuda yüklenirken hata alındıgına dair uyarıcı ex oluştur 
            }
            return null;
        }
    }
}
