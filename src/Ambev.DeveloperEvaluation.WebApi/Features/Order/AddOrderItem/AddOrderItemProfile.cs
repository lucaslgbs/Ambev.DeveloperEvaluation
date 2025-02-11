using Ambev.DeveloperEvaluation.Application.Order.AddOrderItem;
using Ambev.DeveloperEvaluation.Application.Order.CreateOrder;
using Ambev.DeveloperEvaluation.WebApi.Features.Order.CreateOrder;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Order.AddOrderItem
{
    public class AddOrderItemProfile : Profile
    {
        public AddOrderItemProfile()
        {
            CreateMap<AddOrderItemRequest, AddOrderItemCommand>();
        }
    }
}
