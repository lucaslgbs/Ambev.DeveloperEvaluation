using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Order.AddOrderItem
{
    public class AddOrderItemProfile : Profile
    {
        public AddOrderItemProfile()
        {
            CreateMap<AddOrderItemCommand, Domain.Entities.OrderItem>();
        }
    }
}
