using ETicaretAPI.Application.Abstractions.Services.Baskets;
using ETicaretAPI.Application.DTOs.Basket;
using ETicaretAPI.Application.Repositories.BasketItems;
using ETicaretAPI.Application.Repositories.Baskets;
using ETicaretAPI.Application.Repositories.Orders;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Concretes.Baskets
{
    public class BasketAppService : IBasketAppService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly IBasketWriteRepository _basketWriteRepository;
        private readonly IBasketItemWriteRepository _basketItemWriteRepository;
        private readonly IBasketItemReadRepository _basketItemReadRepository;
        private readonly IBasketReadRepository _basketReadRepository;

        public BasketAppService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IOrderReadRepository orderReadRepository, IBasketWriteRepository basketWriteRepository, IBasketItemWriteRepository basketItemWriteRepository, IBasketItemReadRepository basketItemReadRepository, IBasketReadRepository basketReadRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _orderReadRepository = orderReadRepository;
            _basketWriteRepository = basketWriteRepository;
            _basketItemWriteRepository = basketItemWriteRepository;
            _basketItemReadRepository = basketItemReadRepository;
            _basketReadRepository = basketReadRepository;
        }

        private async Task<Basket?> ContextUser()
        {
            var userDetail = _httpContextAccessor?.HttpContext?.User?.Identity?.Name; //giriş yapan kullanicinin user bilgisi
            if (!string.IsNullOrEmpty(userDetail))
            {
                AppUser user = await _userManager.Users
                      .Include(u => u.Baskets)
                     .FirstOrDefaultAsync(u => u.UserName == userDetail);

                //amac orderı olmayan basket bulma

                var _basket = from basket in user.Baskets //kullanıcınn sepetine gidildi
                              join order in _orderReadRepository.Table  //order repositorysi üzerinden ordderlar table gidildi
                              on basket.Id equals order.Id into BasketOrders //sepet içindeki id ile sipariş içindeki id yi eşit olanları getir ve orders ismini ver
                              from order in BasketOrders.DefaultIfEmpty()
                              select new  //basket ve orderlari cagırildi
                              {
                                  Basket = basket,
                                  Order = order
                              };

                Basket? targetBasket = null;
                if (_basket.Any(b => b.Order is null))  //gelen sepette orders null olan varmı
                    targetBasket = _basket.FirstOrDefault(b => b.Order is null)?.Basket; //siparisi null olan sepet verildi

                else //öyle bir sepet yoksada sepet eklensin
                {
                    targetBasket = new();
                    user.Baskets.Add(targetBasket);
                }


                await _basketWriteRepository.SaveAsync(); //yansıtılan sepet veri tabanına gönder
                return targetBasket;
            }
            throw new Exception("Beklenmeyen hata BasketAppService Methods");
        }


        public async Task AddItemToBasketAsync(BasketItemCreateDto dto)
        {
            Basket? basket = await ContextUser();
            if (basket != null)
            {
                BasketItem basketItem = await _basketItemReadRepository.GetSingleAsync(bi => bi.BasketId == basket.Id && bi.ProductId == Guid.Parse(dto.ProductId)); //önceden sepete basket ıd ye uygunnürün eklenmiş güncelleme işlemi yapılması lazım
                                                                                                                                                                     //
                if (basketItem != null) //güncelleme
                {
                    basketItem.Quantity++;
                }
                else
                {
                    await _basketItemWriteRepository.AddAsync(new() //insert işlemi
                    {
                        BasketId = basket.Id,
                        ProductId = Guid.Parse(dto.ProductId),
                        Quantity = dto.Quantity,

                    });
                }
                await _basketItemWriteRepository.SaveAsync();
            }

        }

        public async Task<List<BasketItem>> GetBasketItemsAsync()
        {
            Basket? basket = await ContextUser();
            var basketResult = await _basketReadRepository.Table
                  .Include(b => b.BasketItems)
                  .ThenInclude(p => p.Product)
                  .FirstOrDefaultAsync(b => b.Id == basket.Id);

            return basketResult.BasketItems
                .ToList();
        }

        public async Task RemoveBasketItemAsync(string basketItemId)
        {
            BasketItem? basketItem = await _basketItemReadRepository.GetByIdAsync(basketItemId);
            if (basketItem != null)
            {
                _basketItemWriteRepository.Remove(basketItem);
                await _basketItemWriteRepository.SaveAsync();
            }
        }

        public async Task UpdateQuantityAsync(BasketItemUpdateDto dto)
        {
            BasketItem? basketItem = await _basketItemReadRepository.GetByIdAsync(dto.BasketItemId);
            if (basketItem != null)
            {
                basketItem.Quantity = dto.Quantity;
                await _basketItemWriteRepository.SaveAsync();

            }
        }
    }
}
