using Ambev.DeveloperEvaluation.Application.Order.CancelOrderItem;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Order.CancelOrderItem
{
    public class CancelOrderItemProfile : Profile
    {
        public CancelOrderItemProfile()
        {
            CreateMap<(Guid OrderId, Guid ItemId), CancelOrderItemCommand>()
                .ConstructUsing(ids => new CancelOrderItemCommand(ids.OrderId, ids.ItemId));
        }
    }
}
