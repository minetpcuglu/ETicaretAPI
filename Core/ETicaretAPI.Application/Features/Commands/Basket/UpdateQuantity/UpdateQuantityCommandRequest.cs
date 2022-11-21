using MediatR;

namespace ETicaretAPI.Application.Features.Commands.Basket.UpdateQuantity
{
    public class UpdateQuantityCommandRequest: IRequest<UpdateQuantityCommandResponse>
    {
        //clienttan gelen
        public string BasketItemId { get; set; }
        public int Quantity { get; set; }
    }
}