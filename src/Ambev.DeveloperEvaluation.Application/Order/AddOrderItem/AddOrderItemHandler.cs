using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Order.AddOrderItem
{
    public class AddOrderItemHandler : IRequestHandler<AddOrderItemCommand, AddOrderItemResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public AddOrderItemHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<AddOrderItemResponse> Handle(AddOrderItemCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {request.OrderId} not found");

            if (order.IsCancelled)
                throw new InvalidOperationException($"Order with ID {request.OrderId} is already cancelled");

            var newItem = _mapper.Map<OrderItem>(request);
            order.Items.Add(newItem);

            await _orderRepository.UpdateAsync(order, cancellationToken);

            return new AddOrderItemResponse { Success = true };
        }
    }
}
