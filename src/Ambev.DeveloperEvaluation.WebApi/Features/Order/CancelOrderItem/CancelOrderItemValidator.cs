using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Order.CancelOrderItem
{
    public class CancelOrderItemRequestValidator : AbstractValidator<CancelOrderItemRequest>
    {
        public CancelOrderItemRequestValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order ID is required");
            RuleFor(x => x.ItemId).NotEmpty().WithMessage("Order item ID is required");
        }
    }
}
