using ETicaretAPI.Application.DTOs.Basket;
using ETicaretAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services.Baskets
{
    public interface IBasketAppService
    {
        public Task<List<BasketItem>> GetBasketItemsAsync();
        public Task AddItemToBasketAsync(BasketItemCreateDto dto);
        public Task UpdateQuantityAsync(BasketItemUpdateDto dto);
        public Task RemoveBasketItemAsync(string basketItemId);
    }
}
