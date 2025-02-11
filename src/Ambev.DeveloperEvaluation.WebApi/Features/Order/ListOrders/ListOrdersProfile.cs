using Ambev.DeveloperEvaluation.Application.Order.ListOrders;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Order.ListOrders
{
    public class ListOrdersProfile : Profile
    {
        public ListOrdersProfile()
        {
            CreateMap<ListOrdersRequest, ListOrdersCommand>();
        }
    }
}
