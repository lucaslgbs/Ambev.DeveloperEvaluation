using FluentAssertions;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Order.ListOrders;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Unit.Application.Order
{
    public class ListOrdersHandlerTests
    {
        private readonly IOrderRepository orderRepositoryMock;
        private readonly IMapper mapperMock;
        private readonly ListOrdersHandler handler;
        private readonly Faker faker;

        public ListOrdersHandlerTests()
        {
            orderRepositoryMock = Substitute.For<IOrderRepository>();
            mapperMock = Substitute.For<IMapper>();
            handler = new ListOrdersHandler(orderRepositoryMock, mapperMock);
            faker = new Faker();
        }

        [Fact]
        public async Task HandleShouldReturnOrdersWhenValidRequest()
        {
            // Arrange
            var command = new ListOrdersCommand { Page = 1, PageSize = 10, Search = "Test" };
            var orders = new List<DeveloperEvaluation.Domain.Entities.Order> { new DeveloperEvaluation.Domain.Entities.Order { Id = Guid.NewGuid(), OrderNumber = "12345" } };
            var expectedResults = new List<ListOrdersResult> { new ListOrdersResult { Id = orders[0].Id, OrderNumber = "12345" } };

            orderRepositoryMock.GetPagedOrdersAsync(command.Page, command.PageSize, command.Search, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(orders));

            mapperMock.Map<List<ListOrdersResult>>(orders).Returns(expectedResults);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].OrderNumber.Should().Be("12345");

            await orderRepositoryMock.Received(1).GetPagedOrdersAsync(command.Page, command.PageSize, command.Search, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task HandleShouldReturnEmptyListWhenNoOrdersFound()
        {
            // Arrange
            var command = new ListOrdersCommand { Page = 1, PageSize = 10, Search = "NotFound" };
            orderRepositoryMock.GetPagedOrdersAsync(command.Page, command.PageSize, command.Search, Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(new List<DeveloperEvaluation.Domain.Entities.Order>()));

            mapperMock.Map<List<ListOrdersResult>>(Arg.Any<List<DeveloperEvaluation.Domain.Entities.Order>>()).Returns(new List<ListOrdersResult>());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            await orderRepositoryMock.Received(1).GetPagedOrdersAsync(command.Page, command.PageSize, command.Search, Arg.Any<CancellationToken>());
        }
    }
}
