namespace Ambev.DeveloperEvaluation.WebApi.Features.Order.CancelOrderItem
{
    public class CancelOrderItemRequest
    {
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }
    }

}
