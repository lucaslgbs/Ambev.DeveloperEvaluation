namespace Ambev.DeveloperEvaluation.WebApi.Features.Order.AddOrderItem
{
    public class AddOrderItemRequest
    {
        public Guid OrderId { get; set; }
        public string Product { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
