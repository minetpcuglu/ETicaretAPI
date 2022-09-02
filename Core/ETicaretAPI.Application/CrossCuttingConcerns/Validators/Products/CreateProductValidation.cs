using ETicaretAPI.Application.Requests.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.CrossCuttingConcerns.Validators.Products
{
   public class CreateProductValidation:AbstractValidator<ProductCreateRequest>
    {
        public CreateProductValidation()
        {
            RuleFor(p => p.Name).
                NotEmpty().
                NotNull().
                   WithMessage("Ürün adı boş geçilemez").
                MaximumLength(200).
                MinimumLength(3).
                   WithMessage("Ürün Adı min 3 maks 200 karakter olmalı");

            RuleFor(p => p.UnitInStock)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Stok bilgisi boş geçilemez").
                 Must(s=>s>=0)
                    .WithMessage("Stok bilgisi negatif deger olamaz");

            RuleFor(p => p.Price)
               .NotEmpty()
               .NotNull()
                   .WithMessage("Fiyat bilgisi boş geçilemez").
                Must(s => s >= 0)
                   .WithMessage("Fiyat bilgisi negatif deger olamaz");
        }
    }
}
