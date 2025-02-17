using AutoMapper;
using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Order.GetOrder;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using Bogus;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Tests.Order
{
    public class GetOrderHandlerTests
    {
        private readonly IOrderRepository orderRepositoryMock;
        private readonly IMapper mapperMock;
        private readonly GetOrderHandler handler;
        private readonly Faker faker;

        public GetOrderHandlerTests()
        {
            orderRepositoryMock = Substitute.For<IOrderRepository>();
            mapperMock = Substitute.For<IMapper>();
            handler = new GetOrderHandler(orderRepositoryMock, mapperMock);
            faker = new Faker();
        }

        [Fact]
        public async Task HandleShouldReturnOrderWhenOrderExists()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var command = new GetOrderCommand(orderId);
            var existingOrder = new Domain.Entities.Order { Id = orderId, OrderNumber = "12345" };
            var expectedResult = new GetOrderResult { Id = orderId, OrderNumber = "12345" };

            orderRepositoryMock.GetByIdAsync(orderId, Arg.Any<CancellationToken>()).Returns(Task.FromResult(existingOrder));
            mapperMock.Map<GetOrderResult>(existingOrder).Returns(expectedResult);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(orderId);
            result.OrderNumber.Should().Be("12345");

            await orderRepositoryMock.Received(1).GetByIdAsync(orderId, Arg.Any<CancellationToken>());
            mapperMock.Received(1).Map<GetOrderResult>(existingOrder);
        }

        [Fact]
        public async Task HandleShouldThrowValidationExceptionWhenCommandIsInvalid()
        {
            // Arrange
            var command = new GetOrderCommand(Guid.Empty);

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact]
        public async Task HandleShouldThrowKeyNotFoundExceptionWhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var command = new GetOrderCommand(orderId);

            orderRepositoryMock.GetByIdAsync(orderId, Arg.Any<CancellationToken>()).Returns(Task.FromResult<Domain.Entities.Order>(null));

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage($"Order with ID {orderId} not found");
            await orderRepositoryMock.Received(1).GetByIdAsync(orderId, Arg.Any<CancellationToken>());
        }
    }
}
