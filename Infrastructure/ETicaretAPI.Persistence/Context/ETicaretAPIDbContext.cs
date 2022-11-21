using ETicaretAPI.Domain;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Common;
using ETicaretAPI.Domain.Entities.Files;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Context
{
    public class ETicaretAPIDbContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public ETicaretAPIDbContext( DbContextOptions<ETicaretAPIDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<ProductImageFile> ProductImageFiles { get; set; }
        public DbSet<InvoiceFile> InvoiceFiles { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<Basket> Baskets { get; set; }


        // sepet ile şiparis arasındaki ilişkinin birebir oldugunu belitrmek icin kulllanıldı
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Order>()
                 .HasKey(b => b.Id); //primarykey oldugu belirtildi

            builder.Entity<Basket>()
                .HasOne(b => b.Order)
                .WithOne(o=>o.Basket)
                .HasForeignKey<Order>(b=>b.Id);

            base.OnModelCreating(builder); //base class olarak db context haricinde bir class kullanıldıgı icin bu methoıdu kullanmadan migration yaparsak hata alırız
        }



        //gelen isteklerde insert uptade insert ise createddate update ise updateddate doldur
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //ChangeTracker entityler üzerinden yapılan değişikliği ya da yeni eklenen verinin yakalanmasını sağlayan prop.update işleminde track edilen verileri yakalayıp elde etmemizi saglar
           var datas= ChangeTracker.Entries<BaseEntity>();
            foreach (var data in datas) //gelen datalar yakalancak
            {
                _ = data.State switch //return yapmaya gerek olmadıgı ıcın _ ile döndürmedik
                {
                    EntityState.Added => data.Entity.CreatedDate = DateTime.Now,
                    EntityState.Modified => data.Entity.UpdatedDate = DateTime.Now,
                    _ => DateTime.Now //delete işlemi için yapıldı silinmiş veriler update girmesinn diye
                };
            }
        return await base.SaveChangesAsync(cancellationToken);
        }


    }
}
