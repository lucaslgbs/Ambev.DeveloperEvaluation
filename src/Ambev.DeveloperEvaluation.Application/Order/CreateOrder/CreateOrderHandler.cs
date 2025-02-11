using Ambev.DeveloperEvaluation.Application.Order.ListOrders;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata.Ecma335;

namespace Ambev.DeveloperEvaluation.Application.Order.CreateOrder
{
    public class CreateOrderHandler : OrderBaseHandler, IRequestHandler<CreateOrderCommand, CreateOrderResult>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private ILogger<CreateOrderHandler> _logger;

        public CreateOrderHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<CreateOrderHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating order {OrderNumber}", command.OrderNumber);
            var validator = new CreateOrderValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            command.Items.ForEach(ApplyDiscount);

            var order = _mapper.Map<Domain.Entities.Order>(command);
            var createdOrder = await _orderRepository.CreateAsync(order, cancellationToken);

            _logger.LogInformation("Message Event - rountingKey: CREATED", createdOrder);

            return _mapper.Map<CreateOrderResult>(createdOrder);

        }
    }
}
