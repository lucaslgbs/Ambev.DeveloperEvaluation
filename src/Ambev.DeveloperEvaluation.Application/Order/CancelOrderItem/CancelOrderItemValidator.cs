using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Order.CancelOrderItem
{
    public class CancelOrderItemValidator : AbstractValidator<CancelOrderItemCommand>
    {
        public CancelOrderItemValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order ID is required");
            RuleFor(x => x.ItemId).NotEmpty().WithMessage("Order item ID is required");
        }
    }
}
