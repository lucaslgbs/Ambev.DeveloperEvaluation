using FluentAssertions;
using FluentValidation;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Order.CancelOrderItem;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Order.AddOrderItem;
using Microsoft.Extensions.Logging;
using Bogus;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Application.Order
{
    public class CancelOrderItemHandlerTests
    {
        private readonly IOrderRepository orderRepositoryMock;
        private readonly CancelOrderItemHandler handler;
        private readonly ILogger<CancelOrderItemHandler> loggerMock;
        private readonly Faker faker;

        public CancelOrderItemHandlerTests()
        {
            loggerMock = Substitute.For<ILogger<CancelOrderItemHandler>>();
            orderRepositoryMock = Substitute.For<IOrderRepository>();
            handler = new CancelOrderItemHandler(orderRepositoryMock, loggerMock);
            faker = new Faker();
        }

        [Fact]
        public async Task HandleShouldCancelOrderItemWhenValidRequest()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var command = new CancelOrderItemCommand(orderId, itemId);
            var order = new DeveloperEvaluation.Domain.Entities.Order
            {
                Id = orderId,
                Items = new List<OrderItem>
                {
                    new OrderItem { Id = itemId, IsCancelled = false }
                }
            };

            orderRepositoryMock.GetByIdAsync(orderId, Arg.Any<CancellationToken>()).Returns(Task.FromResult(order));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            order.Items.First().IsCancelled.Should().BeTrue();
            await orderRepositoryMock.Received(1).UpdateAsync(order, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task HandleShouldThrowKeyNotFoundExceptionWhenOrderNotFound()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var command = new CancelOrderItemCommand(orderId, itemId);

            orderRepositoryMock.GetByIdAsync(orderId, Arg.Any<CancellationToken>()).Returns(Task.FromResult<DeveloperEvaluation.Domain.Entities.Order>(null));

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"Order with ID {orderId} not found");
        }
    }
}
