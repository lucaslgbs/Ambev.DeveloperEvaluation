using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using System.Reflection.Metadata.Ecma335;

namespace Ambev.DeveloperEvaluation.Application.Order.CreateOrder
{
    /// <summary>
    /// Handler for processing CreateOrderCommand requests
    /// </summary>
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResult>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public CreateOrderHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateOrderValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            command.Items.ForEach(ApplyDiscount);

            var order = _mapper.Map<Domain.Entities.Order>(command);
            var createdOrder = await _orderRepository.CreateAsync(order, cancellationToken);
            return _mapper.Map<CreateOrderResult>(createdOrder);
        }

        private void ApplyDiscount(CreateOrderItemCommand item)
        {
            if (item.Quantity >= 10)
                item.Discount = (item.UnitPrice * item.Quantity) * 0.2m;
            else if (item.Quantity >= 4)
                item.Discount = (item.UnitPrice * item.Quantity) * 0.1m;
        }
    }
}
