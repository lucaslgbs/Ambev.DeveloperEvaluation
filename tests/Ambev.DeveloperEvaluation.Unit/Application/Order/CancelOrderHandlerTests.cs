using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Order.CancelOrder;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Bogus;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application.Order
{
    public class CancelOrderHandlerTests
    {
        private readonly IOrderRepository orderRepositoryMock;
        private readonly ILogger<CancelOrderHandler> loggerMock;
        private readonly CancelOrderHandler handler;
        private readonly Faker faker;

        public CancelOrderHandlerTests()
        {
            orderRepositoryMock = Substitute.For<IOrderRepository>();
            loggerMock = Substitute.For<ILogger<CancelOrderHandler>>();
            handler = new CancelOrderHandler(orderRepositoryMock, loggerMock);
            faker = new Faker();
        }

        [Fact]
        public async Task HandleShouldCancelOrderWhenValidRequest()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var command = new CancelOrderCommand(orderId);
            var order = new DeveloperEvaluation.Domain.Entities.Order { Id = orderId, IsCancelled = false };

            orderRepositoryMock.GetByIdAsync(orderId, Arg.Any<CancellationToken>()).Returns(Task.FromResult(order));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            order.IsCancelled.Should().BeTrue();
            await orderRepositoryMock.Received(1).UpdateAsync(order, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task HandleShouldThrowKeyNotFoundExceptionWhenOrderNotFound()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var command = new CancelOrderCommand(orderId);

            orderRepositoryMock.GetByIdAsync(orderId, Arg.Any<CancellationToken>()).Returns(Task.FromResult<DeveloperEvaluation.Domain.Entities.Order>(null));

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"Order with ID {orderId} not found");
        }

        [Fact]
        public async Task HandleShouldThrowValidationExceptionWhenInvalidRequest()
        {
            // Arrange
            var command = new CancelOrderCommand(Guid.Empty);

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<FluentValidation.ValidationException>().WithMessage("*Order ID is required*");
        }
    }
}
