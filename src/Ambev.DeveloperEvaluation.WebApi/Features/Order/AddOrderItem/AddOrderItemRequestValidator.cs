using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Order.AddOrderItem
{
    public class AddOrderItemRequestValidator : AbstractValidator<AddOrderItemRequest>
    {
        public AddOrderItemRequestValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order ID is required");
            RuleFor(x => x.Product).NotEmpty().WithMessage("Product name is required");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
            RuleFor(x => x.UnitPrice).GreaterThan(0).WithMessage("Unit price must be greater than 0");
        }
    }
}
