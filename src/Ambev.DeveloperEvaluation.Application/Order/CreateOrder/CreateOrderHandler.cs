using Ambev.DeveloperEvaluation.Application.Order.Event;
using Ambev.DeveloperEvaluation.Application.Order.Services;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Order.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResult>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateOrderHandler> _logger;
        private readonly IDiscountService _discountService;
        private readonly IMediator _mediator;
        private const int MAX_ITEMS = 20;
        private const int MIN_ITEMS = 1;

        public CreateOrderHandler(
            IOrderRepository orderRepository,
            IMapper mapper,
            ILogger<CreateOrderHandler> logger,
            IDiscountService discountService,
            IMediator mediator)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
            _discountService = discountService;
            _mediator = mediator;
        }

        public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating order {OrderNumber}", command.OrderNumber);

            await ValidateCommand(command, cancellationToken);

            ValidateItems(command.Items);

            command.Items.ForEach(_discountService.ApplyDiscount);

            var order = _mapper.Map<Domain.Entities.Order>(command);
            var createdOrder = await _orderRepository.CreateAsync(order, cancellationToken);

            _logger.LogInformation("Publishing OrderCreatedEvent for order {OrderNumber}", createdOrder.OrderNumber);
            await _mediator.Publish(new OrderCreatedEvent(createdOrder.Id, createdOrder.OrderNumber), cancellationToken);

            return _mapper.Map<CreateOrderResult>(createdOrder);
        }

        private void ValidateItems(List<CreateOrderItemCommand> items)
        {
            items.ForEach(i =>
            {
                if (!(i.Quantity >= MIN_ITEMS && i.Quantity <= MAX_ITEMS))
                    throw new ValidationException($"{i.ProductDescription} - Quantity must be between 1 and 20");
            });
        }

        private async Task ValidateCommand(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateOrderValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }
    }
}
