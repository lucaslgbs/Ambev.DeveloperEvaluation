using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Order.CancelOrderItem
{
    public class CancelOrderItemHandler : IRequestHandler<CancelOrderItemCommand, CancelOrderItemResponse>
    {
        private readonly IOrderRepository _orderRepository;

        public CancelOrderItemHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<CancelOrderItemResponse> Handle(CancelOrderItemCommand request, CancellationToken cancellationToken)
        {
            var validator = new CancelOrderItemValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

            if (order == null)
                throw new KeyNotFoundException($"Order with ID {request.OrderId} not found");

            if (order.IsCancelled)
                throw new InvalidOperationException($"Order with ID {request.OrderId} is already cancelled");

            var item = order.Items.FirstOrDefault(i => i.Id == request.ItemId);
            if (item == null)
                throw new KeyNotFoundException($"Order item with ID {request.ItemId} not found");

            item.IsCancelled = true;
            await _orderRepository.UpdateAsync(order, cancellationToken);

            return new CancelOrderItemResponse { Success = true };
        }
    }
}
