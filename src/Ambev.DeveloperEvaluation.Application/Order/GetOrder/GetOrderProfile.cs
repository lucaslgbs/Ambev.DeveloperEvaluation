using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Order.GetOrder
{
    public class GetOrderProfile : Profile
    {
        public GetOrderProfile()
        {
            CreateMap<Domain.Entities.Order, GetOrderResult>();
            CreateMap<Domain.Entities.OrderItem, GetOrderItemResult>();
        }
    }
}
