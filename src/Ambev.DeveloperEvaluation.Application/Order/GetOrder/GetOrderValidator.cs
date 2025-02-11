using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Order.GetOrder
{
    public class GetOrderValidator : AbstractValidator<GetOrderCommand>
    {
        public GetOrderValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("User ID is required");
        }
    }
}