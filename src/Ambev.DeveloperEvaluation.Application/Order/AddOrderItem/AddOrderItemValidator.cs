using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Order.AddOrderItem
{
    public class AddOrderItemValidator : AbstractValidator<AddOrderItemCommand>
    {
        public AddOrderItemValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order ID is required");
            RuleFor(x => x.ProductCode).NotEmpty().WithMessage("Product Code name is required");
            RuleFor(x => x.ProductDescription).NotEmpty().WithMessage("Product Description name is required");
            RuleFor(x => x.UnitPrice).GreaterThan(0).WithMessage("Unit price must be greater than 0");
        }
    }
}
