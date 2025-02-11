using FluentAssertions;
using FluentValidation;
using Moq;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Order.CancelOrderItem;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Unit.Application.Order
{
    public class CancelOrderItemHandlerTests
    {
        private readonly Mock<IOrderRepository> orderRepositoryMock;
        private readonly CancelOrderItemHandler handler;

        public CancelOrderItemHandlerTests()
        {
            orderRepositoryMock = new Mock<IOrderRepository>();
            handler = new CancelOrderItemHandler(orderRepositoryMock.Object);
        }

        [Fact]
        public async Task handleShouldCancelOrderItemWhenValidRequest()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var command = new CancelOrderItemCommand(orderId, itemId);
            var order = new DeveloperEvaluation.Domain.Entities.Order { Id = orderId, Items = new List<OrderItem> { new OrderItem { Id = itemId, IsCancelled = false } } };

            orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            order.Items.First().IsCancelled.Should().BeTrue();
            orderRepositoryMock.Verify(repo => repo.UpdateAsync(order, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task handleShouldThrowKeyNotFoundExceptionWhenOrderNotFound()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var command = new CancelOrderItemCommand(orderId, itemId);

            orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((DeveloperEvaluation.Domain.Entities.Order)null);

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"Order with ID {orderId} not found");
        }
    }
}
