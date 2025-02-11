using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Order.AddOrderItem
{
    public class AddOrderItemValidator : AbstractValidator<AddOrderItemCommand>
    {
        public AddOrderItemValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order ID is required");
            RuleFor(x => x.ProductCode).NotEmpty().WithMessage("Product Code name is required");
            RuleFor(x => x.ProductDescription).NotEmpty().WithMessage("Product Description name is required");
            RuleFor(x => x.Quantity).InclusiveBetween(1, 20).WithMessage("Quantity must be greater than 0 and cannot be greater than 20.");
            RuleFor(x => x.UnitPrice).GreaterThan(0).WithMessage("Unit price must be greater than 0");
        }
    }
}
