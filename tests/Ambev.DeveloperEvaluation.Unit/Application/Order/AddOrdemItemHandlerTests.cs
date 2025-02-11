using FluentAssertions;
using Moq;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Order.AddOrderItem;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Ambev.DeveloperEvaluation.Unit.Application.Order
{
    public class AddOrderItemHandlerTests
    {
        private readonly Mock<IOrderRepository> orderRepositoryMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly AddOrderItemHandler handler;

        public AddOrderItemHandlerTests()
        {
            orderRepositoryMock = new Mock<IOrderRepository>();
            mapperMock = new Mock<IMapper>();
            handler = new AddOrderItemHandler(orderRepositoryMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task handleShouldAddOrderItemWhenValidRequest()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var command = new AddOrderItemCommand
            {
                OrderId = orderId,
                ProductCode = "P001",
                ProductDescription = "Product 1",
                Quantity = 5,
                UnitPrice = 10.0m
            };
            var order = new DeveloperEvaluation.Domain.Entities.Order { Id = orderId, IsCancelled = false };
            var orderItem = new OrderItem { Id = Guid.NewGuid(), ProductCode = "P001", ProductDescription = "Product 01", Quantity = 5, UnitPrice = 10.0m };
            var expectedResult = new AddOrderItemResponse { Success = true };

            orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);
            mapperMock.Setup(mapper => mapper.Map<OrderItem>(command)).Returns(orderItem);
            orderRepositoryMock.Setup(repo => repo.CreateAsync(orderItem, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            orderRepositoryMock.Verify(repo => repo.CreateAsync(orderItem, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task handleShouldThrowKeyNotFoundExceptionWhenOrderNotFound()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var command = new AddOrderItemCommand { OrderId = orderId };

            orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((DeveloperEvaluation.Domain.Entities.Order)null);

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"Order with ID {orderId} not found");
        }

        [Fact]
        public async Task handleShouldThrowInvalidOperationExceptionWhenOrderIsCancelled()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var command = new AddOrderItemCommand { OrderId = orderId };
            var order = new DeveloperEvaluation.Domain.Entities.Order { Id = orderId, IsCancelled = true };

            orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage($"Order with ID {orderId} is already cancelled");
        }
    }
}
