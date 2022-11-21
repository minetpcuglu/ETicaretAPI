namespace ETicaretAPI.Application.Features.Queries.Basket.GetBasketItems
{
    public class GetBasketItemsQueryResponse
    {
        //sepette hangi bilgliler vardır
        public string BasketItemId { get; set; }
        public string ProductName { get; set; }
        public float ProductPrice { get; set; }
        public int Quantity { get; set; }
    }
}