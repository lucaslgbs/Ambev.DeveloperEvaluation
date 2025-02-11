using Ambev.DeveloperEvaluation.Application.Order.CancelOrderItem;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Order.CancelOrderItem
{
    public class CancelOrderItemProfile : Profile
    {
        public CancelOrderItemProfile()
        {
            CreateMap<CancelOrderItemRequest, CancelOrderItemCommand>()
                .ConstructUsing(req => new CancelOrderItemCommand(req.OrderId, req.ItemId));
        }
    }
}
