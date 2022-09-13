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
    public class FileService 
    {
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

    }
}
