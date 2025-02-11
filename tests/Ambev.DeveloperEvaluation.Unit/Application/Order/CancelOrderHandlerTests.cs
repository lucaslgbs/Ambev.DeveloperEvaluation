using FluentAssertions;
using Moq;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Order.CancelOrder;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Unit.Application.Order
{
    public class CancelOrderHandlerTests
    {
        private readonly Mock<IOrderRepository> orderRepositoryMock;
        private readonly Mock<ILogger<CancelOrderHandler>> loggerMock;
        private readonly CancelOrderHandler handler;

        public CancelOrderHandlerTests()
        {
            orderRepositoryMock = new Mock<IOrderRepository>();
            loggerMock = new Mock<ILogger<CancelOrderHandler>>();
            handler = new CancelOrderHandler(orderRepositoryMock.Object, loggerMock.Object);
        }

        [Fact]
        public async Task handleShouldCancelOrderWhenValidRequest()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var command = new CancelOrderCommand(orderId);
            var order = new DeveloperEvaluation.Domain.Entities.Order { Id = orderId, IsCancelled = false };

            orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            order.IsCancelled.Should().BeTrue();
            orderRepositoryMock.Verify(repo => repo.UpdateAsync(order, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task handleShouldThrowKeyNotFoundExceptionWhenOrderNotFound()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var command = new CancelOrderCommand(orderId);

            orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((DeveloperEvaluation.Domain.Entities.Order)null);

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"Order with ID {orderId} not found");
        }

        [Fact]
        public async Task handleShouldThrowValidationExceptionWhenInvalidRequest()
        {
            // Arrange
            var command = new CancelOrderCommand(Guid.Empty);

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>().WithMessage("*Order ID is required*");
        }
    }
}
