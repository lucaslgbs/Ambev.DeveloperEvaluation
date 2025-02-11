using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Order.ListOrders
{
    public class ListOrdersRequestValidator : AbstractValidator<ListOrdersRequest>
    {
        public ListOrdersRequestValidator()
        {
            RuleFor(x => x.Page).GreaterThan(0).WithMessage("Page number must be greater than 0");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("Page size must be greater than 0");
        }
    }
}
