using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Domain.Entities.Common
{
   public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        virtual public DateTime? UpdatedDate { get; set; } //base entityden türeyen tüm classlarda kullanmak istemezsek virtual ile migration ile ekleme zorunlulugunu kaldırıyoruz   istemedigimiz classta override ederek kullanmıyoruz (örn File.cs)
    }
}
