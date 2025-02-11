using Ambev.DeveloperEvaluation.Application.Order.CreateOrder;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Order.CreateOrder
{
    public class CreateOrderProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for CreateOrder feature
        /// </summary>
        public CreateOrderProfile()
        {
            CreateMap<CreateOrderRequest, CreateOrderCommand>();
            CreateMap<CreateOrderItemRequest, CreateOrderItemCommand>();
            CreateMap<CreateOrderResult, CreateOrderResponse>();
            CreateMap<CreateOrderItemResult, CreateOrderItemResponse>();
        }
    }
}
