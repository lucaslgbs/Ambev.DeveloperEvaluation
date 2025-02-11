using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Order.CreateOrder
{
    /// <summary>
    /// Profile for mapping between Application and API CreateOrder responses
    /// </summary>
    public class CreateOrderProfile : Profile
    {
        public CreateOrderProfile()
        {
            CreateMap<CreateOrderCommand, Domain.Entities.Order>();
            CreateMap<CreateOrderItemCommand, Domain.Entities.OrderItem>();
            CreateMap<Domain.Entities.Order, CreateOrderResult>();
            CreateMap< Domain.Entities.OrderItem, CreateOrderItemResult>();
        }
    }
}
