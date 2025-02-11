using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Order.AddOrderItem
{
    public class AddOrderItemHandler : OrderBaseHandler, IRequestHandler<AddOrderItemCommand, AddOrderItemResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private ILogger<AddOrderItemHandler> _logger;

        public AddOrderItemHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<AddOrderItemHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AddOrderItemResponse> Handle(AddOrderItemCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Adding order item to order {OrderId}", command.OrderId);
            var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {command.OrderId} not found");

            if (order.IsCancelled)
                throw new InvalidOperationException($"Order with ID {command.OrderId} is already cancelled");

            command.Discount = CalculateDiscount(command.Quantity, command.Quantity * command.UnitPrice);

            var newItem = _mapper.Map<OrderItem>(command);

            await _orderRepository.CreateAsync(newItem, cancellationToken);

            _logger.LogInformation("Message Event - rountingKey: CREATE_ORDERITEM", newItem);

            return new AddOrderItemResponse { Success = true };
        }
    }
}
