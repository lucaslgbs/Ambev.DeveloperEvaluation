namespace Ambev.DeveloperEvaluation.WebApi.Features.Order.ListOrders
{
    public class ListOrdersResponse
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string Customer { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public bool IsCancelled { get; set; } = false;
        public decimal TotalValue { get; set; }
        public decimal TotalDiscount { get; set; }
        public int TotalItems { get; set; }
    }
}
