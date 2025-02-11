using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Order.CancelOrder
{
    /// <summary>
    /// Handler for processing CancelOrderCommand requests
    /// </summary>
    public class CancelOrderHandler : IRequestHandler<CancelOrderCommand, CancelOrderResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private ILogger<CancelOrderHandler> _logger;

        /// <summary>
        /// Initializes a new instance of CancelOrderHandler
        /// </summary>
        /// <param name="orderRepository">The order repository</param>
        public CancelOrderHandler(IOrderRepository orderRepository, ILogger<CancelOrderHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        /// <summary>
        /// Handles the CancelOrderCommand request
        /// </summary>
        /// <param name="request">The CancelOrder command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The result of the cancel operation</returns>
        public async Task<CancelOrderResponse> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Cancelling order {OrderId}", request.Id);
            var validator = new CancelOrderValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var order = await _orderRepository.GetByIdAsync(request.Id, cancellationToken);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {request.Id} not found");

            order.IsCancelled = true;
            await _orderRepository.UpdateAsync(order, cancellationToken);

            _logger.LogInformation("Message Event - rountingKey: CANCEL_ORDER", order);

            return new CancelOrderResponse { Success = true };
        }
    }

}
