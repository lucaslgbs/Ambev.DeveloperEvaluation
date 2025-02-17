using Ambev.DeveloperEvaluation.Application.Order.CreateOrder;
using Ambev.DeveloperEvaluation.Application.Order.Services;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Order.AddOrderItem
{
    public class AddOrderItemHandler : IRequestHandler<AddOrderItemCommand, AddOrderItemResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDiscountService _discountService;
        private readonly IMapper _mapper;
        private ILogger<AddOrderItemHandler> _logger;
        private const int MAX_ITEMS = 20;
        private const int MIN_ITEMS = 1;

        public AddOrderItemHandler(IOrderRepository orderRepository,
            IMapper mapper,
            ILogger<AddOrderItemHandler> logger,
            IDiscountService discountService)
        {
            _orderRepository = orderRepository;
            _discountService = discountService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AddOrderItemResponse> Handle(AddOrderItemCommand command, CancellationToken cancellationToken)
        {
            await ValidateCommand(command, cancellationToken);

            ValidateItems(command);

            _logger.LogInformation("Adding order item to order {OrderId}", command.OrderId);
            var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

            if (order == null)
                throw new KeyNotFoundException($"Order with ID {command.OrderId} not found");

            if (order.IsCancelled)
                throw new InvalidOperationException($"Order with ID {command.OrderId} is already cancelled");

            command.Discount = _discountService.ApplyDiscount(command.Quantity, command.UnitPrice);

            var newItem = _mapper.Map<OrderItem>(command);

            await _orderRepository.CreateAsync(newItem, cancellationToken);

            _logger.LogInformation("Message Event - rountingKey: CREATE_ORDERITEM", newItem);

            return new AddOrderItemResponse { Success = true };
        }

        private void ValidateItems(AddOrderItemCommand item)
        {
            if (!(item.Quantity >= MIN_ITEMS && item.Quantity <= MAX_ITEMS))
                throw new ValidationException($"{item.ProductDescription} - Quantity must be between 1 and 20");
        }

        private async Task ValidateCommand(AddOrderItemCommand command, CancellationToken cancellationToken)
        {
            var validator = new AddOrderItemValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }


    }
}
