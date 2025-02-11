using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using Ambev.DeveloperEvaluation.WebApi.Features.Order.ListOrders;
using Ambev.DeveloperEvaluation.WebApi.Features.Order.GetOrder;
using Ambev.DeveloperEvaluation.WebApi.Features.Order.CreateOrder;
using Ambev.DeveloperEvaluation.WebApi.Features.Order.CancelOrder;
using Ambev.DeveloperEvaluation.WebApi.Features.Order.CancelOrderItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Order.AddOrderItem;
using Ambev.DeveloperEvaluation.Application.Order.CancelOrder;

namespace Ambev.DeveloperEvaluation.WebApi.Controllers
{
    /// <summary>
    /// Controller responsible for handling order-related operations.
    /// </summary>
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR instance for handling requests.</param>
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves a list of orders with pagination and optional search.
        /// </summary>
        /// <param name="request">List orders request parameters.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of orders.</returns>
        [HttpGet]
        public async Task<ActionResult<List<ListOrdersResponse>>> ListOrders([FromQuery] ListOrdersRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(request, cancellationToken));
        }

        /// <summary>
        /// Retrieves details of a specific order.
        /// </summary>
        /// <param name="id">Order ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Order details.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetOrderResponse>> GetOrder(Guid id, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new GetOrderRequest { Id = id }, cancellationToken));
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="request">Create order request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Created order details.</returns>
        [HttpPost]
        public async Task<ActionResult<CreateOrderResponse>> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(request, cancellationToken));
        }

        /// <summary>
        /// Cancels an order.
        /// </summary>
        /// <param name="id">Order ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Cancellation result.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<CancelOrderResponse>> CancelOrder(Guid id, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(new CancelOrderRequest { Id = id }, cancellationToken));
        }

        /// <summary>
        /// Cancels a specific order item.
        /// </summary>
        /// <param name="request">Cancel order item request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Cancellation result.</returns>
        [HttpDelete("{orderId}/items/{itemId}")]
        public async Task<ActionResult> CancelOrderItem([FromBody] CancelOrderItemRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(request, cancellationToken));
        }

        /// <summary>
        /// Adds an item to an existing order.
        /// </summary>
        /// <param name="request">Add order item request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        [HttpPost("{id}/items")]
        public async Task<ActionResult> AddOrderItem([FromBody] AddOrderItemRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _mediator.Send(request, cancellationToken));
        }
    }
}
