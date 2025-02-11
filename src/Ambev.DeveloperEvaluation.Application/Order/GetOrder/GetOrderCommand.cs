using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Order.GetOrder
{
    public class GetOrderCommand : IRequest<GetOrderResult>
    {
        public Guid Id { get; set; }

        public GetOrderCommand(Guid id)
        {
            Id = id;
        }
    }
}
