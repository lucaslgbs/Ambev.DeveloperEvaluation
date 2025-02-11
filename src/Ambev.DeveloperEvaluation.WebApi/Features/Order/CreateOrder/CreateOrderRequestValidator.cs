using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Order.CreateOrder
{
    /// <summary>
    /// Validator for CreateOrderRequest that defines validation rules for order creation.
    /// </summary>
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(order => order.OrderNumber).NotEmpty().Length(3, 50);
            RuleFor(order => order.Customer).NotEmpty();
            RuleForEach(order => order.Items).SetValidator(new OrderItemValidator());
        }
    }

    /// <summary>
    /// Validator for OrderItem ensuring quantity and pricing rules.
    /// </summary>
    public class OrderItemValidator : AbstractValidator<CreateOrderItemRequest>
    {
        public OrderItemValidator()
        {
            RuleFor(item => item.ProductDescription).NotEmpty();
            RuleFor(item => item.ProductCode).NotEmpty();
            RuleFor(item => item.Quantity).InclusiveBetween(1, 20);
            RuleFor(item => item.UnitPrice).GreaterThan(0);
        }
    }
}
