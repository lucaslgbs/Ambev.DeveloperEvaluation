using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Order.AddOrderItem;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Bogus;
using NSubstitute;
using Ambev.DeveloperEvaluation.Application.Order.Services;

namespace Ambev.DeveloperEvaluation.Unit.Application.Order
{
    public class AddOrderItemHandlerTests
    {
        private readonly IOrderRepository orderRepositoryMock;
        private readonly IDiscountService _discountService;
        private readonly IMapper mapperMock;
        private readonly ILogger<AddOrderItemHandler> loggerMock;
        private readonly AddOrderItemHandler handler;
        private readonly Faker faker;

        public AddOrderItemHandlerTests()
        {
            orderRepositoryMock = Substitute.For<IOrderRepository>();
            mapperMock = Substitute.For<IMapper>();
            loggerMock = Substitute.For<ILogger<AddOrderItemHandler>>();
            _discountService = new DiscountService();
            handler = new AddOrderItemHandler(orderRepositoryMock, mapperMock, loggerMock, _discountService);
            faker = new Faker();
        }

        [Fact]
        public async Task HandleShouldAddOrderItemWhenValidRequest()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var command = new Faker<AddOrderItemCommand>()
                .RuleFor(c => c.OrderId, orderId)
                .RuleFor(c => c.ProductCode, f => f.Commerce.Ean13())
                .RuleFor(c => c.ProductDescription, f => f.Commerce.ProductName())
                .RuleFor(c => c.Quantity, f => f.Random.Int(1, 20))
                .RuleFor(c => c.UnitPrice, f => f.Random.Decimal(1, 1000))
                .Generate();

            var order = new DeveloperEvaluation.Domain.Entities.Order { Id = orderId, IsCancelled = false };
            var orderItem = new Faker<OrderItem>()
                .RuleFor(i => i.Id, Guid.NewGuid())
                .RuleFor(i => i.ProductCode, command.ProductCode)
                .RuleFor(i => i.ProductDescription, command.ProductDescription)
                .RuleFor(i => i.Quantity, command.Quantity)
                .RuleFor(i => i.UnitPrice, command.UnitPrice)
                .Generate();

            orderRepositoryMock.GetByIdAsync(orderId, Arg.Any<CancellationToken>()).Returns(Task.FromResult(order));
            mapperMock.Map<OrderItem>(command).Returns(orderItem);
            orderRepositoryMock.CreateAsync(orderItem, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            await orderRepositoryMock.Received(1).CreateAsync(orderItem, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task HandleShouldThrowKeyNotFoundExceptionWhenOrderNotFound()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var command = new Faker<AddOrderItemCommand>()
                                    .RuleFor(c => c.OrderId, orderId)
                                    .RuleFor(c => c.ProductCode, f => f.Commerce.Ean13())
                                    .RuleFor(c => c.ProductDescription, f => f.Commerce.ProductName())
                                    .RuleFor(c => c.Quantity, f => f.Random.Int(1, 20))
                                    .RuleFor(c => c.UnitPrice, f => f.Random.Decimal(1, 1000))
                                    .Generate();

            orderRepositoryMock.GetByIdAsync(orderId, Arg.Any<CancellationToken>()).Returns(Task.FromResult<DeveloperEvaluation.Domain.Entities.Order>(null));

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"Order with ID {orderId} not found");
        }

        [Fact]
        public async Task HandleShouldThrowInvalidOperationExceptionWhenOrderIsCancelled()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var command = new Faker<AddOrderItemCommand>()
                        .RuleFor(c => c.OrderId, orderId)
                        .RuleFor(c => c.ProductCode, f => f.Commerce.Ean13())
                        .RuleFor(c => c.ProductDescription, f => f.Commerce.ProductName())
                        .RuleFor(c => c.Quantity, f => f.Random.Int(1, 20))
                        .RuleFor(c => c.UnitPrice, f => f.Random.Decimal(1, 1000))
                        .Generate();

            var order = new DeveloperEvaluation.Domain.Entities.Order { Id = orderId, IsCancelled = true };

            orderRepositoryMock.GetByIdAsync(orderId, Arg.Any<CancellationToken>()).Returns(Task.FromResult(order));

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage($"Order with ID {orderId} is already cancelled");
        }

        [Fact]
        public void ValidatorShouldFailWhenInvalidRequest()
        {
            // Arrange
            var validator = new AddOrderItemValidator();
            var command = new AddOrderItemCommand();

            // Act
            var result = validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }
    }
}
