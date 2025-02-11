namespace Ambev.DeveloperEvaluation.Application.Order.CreateOrder
{
    public class CreateOrderResult
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string Customer { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public List<CreateOrderItemResult> Items { get; set; } = new();
        public bool IsCancelled { get; set; } = false;
    }

    public class CreateOrderItemResult
    {
        public Guid Id { get; set; }
        public string ProductCode { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public decimal UnitPrice { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
    }
}
