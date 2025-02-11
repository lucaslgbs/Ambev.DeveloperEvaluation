using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Order.GetOrder
{
    public class GetOrderRequestValidator : AbstractValidator<GetOrderRequest>
    {
        public GetOrderRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Order ID is required");
        }
    }
}