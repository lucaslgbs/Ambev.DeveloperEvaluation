using Ambev.DeveloperEvaluation.Application.Order.GetOrder;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Order.GetOrder
{
    public class GetOrderProfile : Profile
    {
        public GetOrderProfile()
        {
            CreateMap<Guid, Application.Users.GetUser.GetUserCommand>()
            .ConstructUsing(id => new Application.Users.GetUser.GetUserCommand(id));
            CreateMap<GetOrderResult, GetOrderResponse>();
            CreateMap<GetOrderItemResult, GetOrderItemResponse>();
        }
    }
}