using Ambev.DeveloperEvaluation.Application.Order.CreateOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Order
{
    public class OrderBaseHandler
    {
        protected void ApplyDiscount(CreateOrderItemCommand item)
        {
            item.Discount = CalculateDiscount(item.Quantity, item.Quantity * item.UnitPrice);
        }

        protected decimal CalculateDiscount(int quantity, decimal totalPrice)
        {
            if (quantity >= 10)
                return totalPrice * 0.2m;
            else if (quantity >= 4)
                return totalPrice * 0.1m;
            return totalPrice;
        }
    }
}
