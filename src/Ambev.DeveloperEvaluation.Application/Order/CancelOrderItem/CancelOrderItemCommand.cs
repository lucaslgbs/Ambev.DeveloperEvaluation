using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Order.CancelOrderItem
{
    public class CancelOrderItemCommand : IRequest<CancelOrderItemResponse>
    {
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }

        public CancelOrderItemCommand(Guid orderId, Guid itemId)
        {
            OrderId = orderId;
            ItemId = itemId;
        }
    }

}
