using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Order.ListOrders
{
    public class ListOrdersProfile : Profile
    {
        public ListOrdersProfile()
        {
            CreateMap<Domain.Entities.Order, ListOrdersResult>()
                .ForMember(dest => dest.TotalValue, opt => opt.MapFrom(src => src.Items.Where(x => !x.IsCancelled).Sum(i => i.UnitPrice * i.Quantity)))
                .ForMember(dest => dest.TotalDiscount, opt => opt.MapFrom(src => src.Items.Where(x => !x.IsCancelled).Sum(i => i.Discount)))
                .ForMember(dest => dest.TotalItems, opt => opt.MapFrom(src => src.Items.Where(x => !x.IsCancelled).ToList().Count));
        }
    }
}
