using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Order.CancelOrder
{
    /// <summary>
    /// Handler for processing CancelOrderCommand requests
    /// </summary>
    public class CancelOrderHandler : IRequestHandler<CancelOrderCommand, CancelOrderResponse>
    {
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// Initializes a new instance of CancelOrderHandler
        /// </summary>
        /// <param name="orderRepository">The order repository</param>
        public CancelOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Handles the CancelOrderCommand request
        /// </summary>
        /// <param name="request">The CancelOrder command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The result of the cancel operation</returns>
        public async Task<CancelOrderResponse> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var validator = new CancelOrderValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var order = await _orderRepository.GetByIdAsync(request.Id, cancellationToken);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {request.Id} not found");

            order.IsCancelled = true;
            await _orderRepository.UpdateAsync(order, cancellationToken);

            return new CancelOrderResponse { Success = true };
        }
    }

}
