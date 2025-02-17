using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Order.CreateOrder;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Bogus;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Order.Services;
using MediatR;

namespace Ambev.DeveloperEvaluation.Unit.Application.Order
{
    public class CreateOrderHandlerTests
    {
        private readonly IOrderRepository orderRepositoryMock;
        private readonly IDiscountService _discountService;
        private readonly IMapper mapperMock;
        private readonly CreateOrderHandler handler;
        private readonly ILogger<CreateOrderHandler> loggerMock;
        private readonly IMediator mediatorMock;
        private readonly Faker faker;

        public CreateOrderHandlerTests()
        {
            orderRepositoryMock = Substitute.For<IOrderRepository>();
            mapperMock = Substitute.For<IMapper>();
            loggerMock = Substitute.For<ILogger<CreateOrderHandler>>();
            _discountService = new DiscountService();
            mediatorMock = Substitute.For<IMediator>();
            handler = new CreateOrderHandler(orderRepositoryMock, mapperMock, loggerMock, _discountService, mediatorMock);
            faker = new Faker();
        }

        [Fact]
        public async Task HandleShouldCreateOrderWhenValidRequest()
        {
            // Arrange
            var command = new Faker<CreateOrderCommand>()
                .RuleFor(c => c.OrderNumber, f => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.OrderDate, f => f.Date.Past())
                .RuleFor(c => c.Customer, f => f.Company.CompanyName())
                .RuleFor(c => c.Branch, f => f.Address.City())
                .RuleFor(c => c.Items, f => new List<CreateOrderItemCommand>
                {
                    new CreateOrderItemCommand { ProductCode = "P001", ProductDescription = "Product 1", Quantity = 2, UnitPrice = 10.0m },
                    new CreateOrderItemCommand { ProductCode = "P002", ProductDescription = "Product 2", Quantity = 5, UnitPrice = 20.0m },
                    new CreateOrderItemCommand { ProductCode = "P003", ProductDescription = "Product 3", Quantity = 10, UnitPrice = 30.0m }
                })
                .Generate();

            var order = new DeveloperEvaluation.Domain.Entities.Order { Id = Guid.NewGuid(), OrderNumber = command.OrderNumber };
            var expectedResult = new CreateOrderResult { Id = order.Id, OrderNumber = command.OrderNumber };

            mapperMock.Map<DeveloperEvaluation.Domain.Entities.Order>(command).Returns(order);
            orderRepositoryMock.CreateAsync(order, Arg.Any<CancellationToken>()).Returns(Task.FromResult(order));
            mapperMock.Map<CreateOrderResult>(order).Returns(expectedResult);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(order.Id);
            result.OrderNumber.Should().Be(command.OrderNumber);

            await orderRepositoryMock.Received(1).CreateAsync(order, Arg.Any<CancellationToken>());
            mapperMock.Received(1).Map<CreateOrderResult>(order);

            // Validate discounts
            command.Items[0].Discount.Should().Be(0);
            command.Items[1].Discount.Should().Be((command.Items[1].UnitPrice * command.Items[1].Quantity) * 0.1m);
            command.Items[2].Discount.Should().Be((command.Items[2].UnitPrice * command.Items[2].Quantity) * 0.2m);
        }

        [Fact]
        public async Task HandleShouldThrowValidationExceptionWhenInvalidRequest()
        {
            // Arrange
            var command = new CreateOrderCommand();

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<FluentValidation.ValidationException>();
        }
    }
}
