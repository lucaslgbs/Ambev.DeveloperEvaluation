﻿using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Order.CancelOrder
{
    public class CancelOrderValidator : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Order ID is required");
        }
    }
}