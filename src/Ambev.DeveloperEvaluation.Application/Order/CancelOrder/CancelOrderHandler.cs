using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Order.CancelOrder
{
    /// <summary>
    /// Handler for processing CancelOrderCommand requests
    /// </summary>
    public class CancelOrderHandler : IRequestHandler<CancelOrderCommand, CancelOrderResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<CancelOrderHandler> _logger;

        public CancelOrderHandler(IOrderRepository orderRepository, ILogger<CancelOrderHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<CancelOrderResponse> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Cancelling order {OrderId}", request.Id);

            await ValidateRequest(request, cancellationToken);

            var order = await _orderRepository.GetByIdAsync(request.Id, cancellationToken);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {request.Id} not found");

            order.IsCancelled = true;
            await _orderRepository.UpdateAsync(order, cancellationToken);

            _logger.LogInformation("Message Event - rountingKey: CANCEL_ORDER", order);

            return new CancelOrderResponse { Success = true };
        }

        private async Task ValidateRequest(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var validator = new CancelOrderValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }
    }
}
