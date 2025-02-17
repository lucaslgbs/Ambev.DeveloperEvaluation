using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Order.Event
{
    public class OrderCreatedEvent : INotification
    {
        public Guid OrderId { get; }
        public string OrderNumber { get; }

        public OrderCreatedEvent(Guid orderId, string orderNumber)
        {
            OrderId = orderId;
            OrderNumber = orderNumber;
        }
    }
}
