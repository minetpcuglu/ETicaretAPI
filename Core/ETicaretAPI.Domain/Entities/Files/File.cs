﻿using ETicaretAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Domain.Entities.Files
{
    //Table Per Hierarchy yaklaşımı=> hiyerarşik olarak entity fark etmeksizin tüm türler tek bir tabloya yansıtılacak ve aralarındaki fark ‘Discriminator’ kolonu sayesinde ayrıştırılacaktır. (File ProductImageFile InvoiceFile Hepsi tek bir tabloda)
   public class File:BaseEntity
    {
        public string FileName { get; set; }
        public string Path { get; set; }


        [NotMapped]
        public override DateTime? UpdatedDate { get => base.UpdatedDate; set => base.UpdatedDate = value; } 
        //updated date mig edilsede gelmez
    }
}
