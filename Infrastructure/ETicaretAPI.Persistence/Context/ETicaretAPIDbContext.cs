using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Context
{
    public class ETicaretAPIDbContext : DbContext
    {
        public ETicaretAPIDbContext( DbContextOptions<ETicaretAPIDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }


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
                    EntityState.Modified => data.Entity.UpdatedDate = DateTime.Now
                };
            }
        return await base.SaveChangesAsync(cancellationToken);
        }


    }
}
