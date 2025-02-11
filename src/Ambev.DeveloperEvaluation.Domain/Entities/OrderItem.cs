using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class OrderItem : BaseEntity
    {
        public string ProductCode { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public decimal UnitPrice { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
        public Guid OrderId { get; set; }
        public bool IsCancelled { get; set; } = false;
        public virtual Order Order { get; set; } = null!;
    }
}
