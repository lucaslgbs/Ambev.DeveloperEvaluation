using Ambev.DeveloperEvaluation.Application.Order.CreateOrder;

namespace Ambev.DeveloperEvaluation.Application.Order.Services
{
    public interface IDiscountService
    {
        void ApplyDiscount(CreateOrderItemCommand item);
        decimal ApplyDiscount(int quantity, decimal unitPrice);
    }

    public class DiscountService : IDiscountService
    {
        public void ApplyDiscount(CreateOrderItemCommand item)
        {
            item.Discount = ApplyDiscount(item.Quantity, item.UnitPrice);
        }

        public decimal ApplyDiscount(int quantity, decimal unitPrice)
        {
            if (quantity >= 10)
                return (unitPrice * quantity) * 0.2m;
            else if (quantity >= 4)
                return (unitPrice * quantity) * 0.1m;
            else
                return 0;
        }
    }

}
