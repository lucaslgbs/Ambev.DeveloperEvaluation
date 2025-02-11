using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Order.ListOrders
{
    public class ListOrdersValidator : AbstractValidator<ListOrdersCommand>
    {
        public ListOrdersValidator()
        {
            RuleFor(x => x.Page).GreaterThan(0).WithMessage("Page number must be greater than 0");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("Page size must be greater than 0");
        }
    }
}
