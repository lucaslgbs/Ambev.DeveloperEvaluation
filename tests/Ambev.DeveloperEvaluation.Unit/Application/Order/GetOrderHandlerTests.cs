using AutoMapper;
using FluentAssertions;
using Moq;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Order.GetOrder;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Tests.Order
{
    public class GetOrderHandlerTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetOrderHandler _handler;

        public GetOrderHandlerTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetOrderHandler(_orderRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task handleShouldReturnOrderWhenOrderExists()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var command = new GetOrderCommand(orderId);
            var existingOrder = new Domain.Entities.Order { Id = orderId, OrderNumber = "12345" };
            var expectedResult = new GetOrderResult { Id = orderId, OrderNumber = "12345" };

            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingOrder);

            _mapperMock.Setup(mapper => mapper.Map<GetOrderResult>(existingOrder))
                .Returns(expectedResult);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(orderId);
            result.OrderNumber.Should().Be("12345");

            _orderRepositoryMock.Verify(repo => repo.GetByIdAsync(orderId, It.IsAny<CancellationToken>()), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<GetOrderResult>(existingOrder), Times.Once);
        }

        [Fact]
        public async Task handleShouldThrowValidationExceptionWhenCommandIsInvalid()
        {
            // Arrange
            var command = new GetOrderCommand(Guid.Empty);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task handleShouldThrowKeyNotFoundExceptionWhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var command = new GetOrderCommand(orderId);

            _orderRepositoryMock.Setup(repo => repo.GetByIdAsync(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Order)null);

            // Act
            var act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"Order with ID {orderId} not found");
            _orderRepositoryMock.Verify(repo => repo.GetByIdAsync(orderId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
