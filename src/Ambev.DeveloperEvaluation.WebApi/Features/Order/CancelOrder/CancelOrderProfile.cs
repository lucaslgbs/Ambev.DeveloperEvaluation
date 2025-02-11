using Ambev.DeveloperEvaluation.Application.Order.CancelOrder;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Order.CancelOrder
{
    public class CancelOrderProfile : Profile
    {
        public CancelOrderProfile()
        {
            CreateMap<Guid, CancelOrderCommand>()
            .ConstructUsing(id => new CancelOrderCommand(id));
        }
    }
}
