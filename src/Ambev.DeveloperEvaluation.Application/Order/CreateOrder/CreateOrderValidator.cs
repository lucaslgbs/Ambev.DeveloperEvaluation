using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Order.CreateOrder
{
    /// <summary>
    /// Validator for CreateOrderCommand that defines validation rules for order creation command.
    /// </summary>
    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator()
        {
            RuleFor(order => order.OrderNumber).NotEmpty().Length(3, 50);
            RuleFor(order => order.Customer).NotEmpty();
            RuleForEach(order => order.Items).SetValidator(new OrderItemValidator());
        }
    }

    /// <summary>
    /// Validator for OrderItemCommand ensuring quantity and pricing rules.
    /// </summary>
    public class OrderItemValidator : AbstractValidator<CreateOrderItemCommand>
    {
        public OrderItemValidator()
        {
            RuleFor(item => item.ProductCode).NotEmpty();
            RuleFor(item => item.ProductDescription).NotEmpty();
            RuleFor(item => item.Quantity).InclusiveBetween(1, 20);
            RuleFor(item => item.UnitPrice).GreaterThan(0);
            RuleFor(item => item.Discount).GreaterThanOrEqualTo(0);
        }
    }
}
