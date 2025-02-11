using FluentAssertions;
using Moq;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Order.CreateOrder;
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
    public class CreateOrderHandlerTests
    {
        private readonly Mock<IOrderRepository> orderRepositoryMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly CreateOrderHandler handler;

        public CreateOrderHandlerTests()
        {
            orderRepositoryMock = new Mock<IOrderRepository>();
            mapperMock = new Mock<IMapper>();
            handler = new CreateOrderHandler(orderRepositoryMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task handleShouldCreateOrderWhenValidRequest()
        {
            // Arrange
            var command = new CreateOrderCommand
            {
                OrderNumber = "12345",
                OrderDate = DateTime.UtcNow,
                Customer = "Test Customer",
                Branch = "Test Branch",
                Items = new List<CreateOrderItemCommand>
                {
                    new CreateOrderItemCommand { ProductCode = "P001", ProductDescription = "Product 1", Quantity = 2, UnitPrice = 10.0m },
                    new CreateOrderItemCommand { ProductCode = "P002", ProductDescription = "Product 2", Quantity = 5, UnitPrice = 20.0m },
                    new CreateOrderItemCommand { ProductCode = "P003", ProductDescription = "Product 3", Quantity = 10, UnitPrice = 30.0m }
                }
            };

            var order = new DeveloperEvaluation.Domain.Entities.Order { Id = Guid.NewGuid(), OrderNumber = "12345" };
            var expectedResult = new CreateOrderResult { Id = order.Id, OrderNumber = "12345" };

            mapperMock.Setup(mapper => mapper.Map<DeveloperEvaluation.Domain.Entities.Order>(command)).Returns(order);
            orderRepositoryMock.Setup(repo => repo.CreateAsync(order, It.IsAny<CancellationToken>())).ReturnsAsync(order);
            mapperMock.Setup(mapper => mapper.Map<CreateOrderResult>(order)).Returns(expectedResult);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(order.Id);
            result.OrderNumber.Should().Be("12345");

            orderRepositoryMock.Verify(repo => repo.CreateAsync(order, It.IsAny<CancellationToken>()), Times.Once);
            mapperMock.Verify(mapper => mapper.Map<CreateOrderResult>(order), Times.Once);

            // Validate discounts
            command.Items[0].Discount.Should().Be(command.Items[0].UnitPrice * command.Items[0].Quantity);
            command.Items[1].Discount.Should().Be(command.Items[1].UnitPrice * command.Items[1].Quantity * 0.1m);
            command.Items[2].Discount.Should().Be(command.Items[2].UnitPrice * command.Items[2].Quantity * 0.2m);
        }

        [Fact]
        public async Task handleShouldThrowValidationExceptionWhenInvalidRequest()
        {
            // Arrange
            var command = new CreateOrderCommand();

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }
    }
}
