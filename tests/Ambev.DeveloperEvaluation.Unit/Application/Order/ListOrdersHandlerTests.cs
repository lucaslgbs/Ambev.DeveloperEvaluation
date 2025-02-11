using FluentAssertions;
using Moq;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Order.ListOrders;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Unit.Application.Order
{
    public class ListOrdersHandlerTests
    {
        private readonly Mock<IOrderRepository> orderRepositoryMock;
        private readonly Mock<IMapper> mapperMock;
        private readonly ListOrdersHandler handler;

        public ListOrdersHandlerTests()
        {
            orderRepositoryMock = new Mock<IOrderRepository>();
            mapperMock = new Mock<IMapper>();
            handler = new ListOrdersHandler(orderRepositoryMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task handleShouldReturnOrdersWhenValidRequest()
        {
            // Arrange
            var command = new ListOrdersCommand { Page = 1, PageSize = 10, Search = "Test" };
            var orders = new List<DeveloperEvaluation.Domain.Entities.Order> { new DeveloperEvaluation.Domain.Entities.Order { Id = Guid.NewGuid(), OrderNumber = "12345" } };
            var expectedResults = new List<ListOrdersResult> { new ListOrdersResult { Id = orders[0].Id, OrderNumber = "12345" } };

            orderRepositoryMock.Setup(repo => repo.GetPagedOrdersAsync(command.Page, command.PageSize, command.Search, It.IsAny<CancellationToken>()))
                .ReturnsAsync(orders);

            mapperMock.Setup(mapper => mapper.Map<List<ListOrdersResult>>(orders))
                .Returns(expectedResults);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].OrderNumber.Should().Be("12345");

            orderRepositoryMock.Verify(repo => repo.GetPagedOrdersAsync(command.Page, command.PageSize, command.Search, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task handleShouldReturnEmptyListWhenNoOrdersFound()
        {
            // Arrange
            var command = new ListOrdersCommand { Page = 1, PageSize = 10, Search = "NotFound" };
            orderRepositoryMock.Setup(repo => repo.GetPagedOrdersAsync(command.Page, command.PageSize, command.Search, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<DeveloperEvaluation.Domain.Entities.Order>());

            mapperMock.Setup(mapper => mapper.Map<List<ListOrdersResult>>(It.IsAny<List<DeveloperEvaluation.Domain.Entities.Order>>()))
                .Returns(new List<ListOrdersResult>());

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();

            orderRepositoryMock.Verify(repo => repo.GetPagedOrdersAsync(command.Page, command.PageSize, command.Search, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
