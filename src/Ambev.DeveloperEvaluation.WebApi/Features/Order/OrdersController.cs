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
using Ambev.DeveloperEvaluation.Application.Order.CreateOrder;
using Ambev.DeveloperEvaluation.WebApi.Common;
using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Order.CancelOrderItem;
using Ambev.DeveloperEvaluation.Application.Order.AddOrderItem;
using Ambev.DeveloperEvaluation.Application.Order.GetOrder;
using Ambev.DeveloperEvaluation.Application.Order.ListOrders;

namespace Ambev.DeveloperEvaluation.WebApi.Controllers
{
    /// <summary>
    /// Controller responsible for handling order-related operations.
    /// </summary>
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderController"/> class.
        /// </summary>
        /// <param name="mediator">MediatR instance for handling requests.</param>
        public OrdersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a list of orders with pagination and optional search.
        /// </summary>
        /// <param name="request">List orders request parameters.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A list of orders.</returns>
        [HttpGet]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<List<ListOrdersResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListOrders([FromQuery] ListOrdersRequest request, CancellationToken cancellationToken)
        {
            var validator = new ListOrdersRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<ListOrdersCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<List<ListOrdersResponse>>
            {
                Success = true,
                Message = "Orders retrieved successfully",
                Data = _mapper.Map<List<ListOrdersResponse>>(response)
            });
        }

        /// <summary>
        /// Retrieves details of a specific order.
        /// </summary>
        /// <param name="id">Order ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Order details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetOrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrder([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new GetOrderRequest { Id = id };
            var validator = new GetOrderRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<GetOrderCommand>(request.Id);
            var response = await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<GetOrderResponse>
            {
                Success = true,
                Message = "Order retrieved successfully",
                Data = _mapper.Map<GetOrderResponse>(response)
            });
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="request">Create order request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Created order details.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateOrderResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var validator = new CreateOrderRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<CreateOrderCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);

            return Created(string.Empty, new ApiResponseWithData<CreateOrderResponse>
            {
                Success = true,
                Message = "Order created successfully",
                Data = _mapper.Map<CreateOrderResponse>(response)
            });
        }

        /// <summary>
        /// Cancels an order.
        /// </summary>
        /// <param name="id">Order ID.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Cancellation result.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelOrder([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new CancelOrderRequest { Id = id };
            var validator = new CancelOrderRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<CancelOrderCommand>(request.Id);
            await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Order canceled successfully"
            });
        }

        /// <summary>
        /// Cancels a specific order item.
        /// </summary>
        /// <param name="request">Cancel order item request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Cancellation result.</returns>
        [HttpDelete("{orderId}/items/{itemId}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelOrderItem([FromRoute] Guid orderId, [FromRoute] Guid itemId, CancellationToken cancellationToken)
        {
            var request = new CancelOrderItemRequest { OrderId = orderId, ItemId = itemId };
            var validator = new CancelOrderItemRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<CancelOrderItemCommand>(request);
            await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Order item canceled successfully"
            });
        }

        /// <summary>
        /// Adds an item to an existing order.
        /// </summary>
        /// <param name="request">Add order item request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        [HttpPost("{orderId}/items")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddOrderItem([FromRoute] Guid orderId, [FromBody] AddOrderItemRequest request, CancellationToken cancellationToken)
        {
            request.OrderId = orderId;
            var validator = new AddOrderItemRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<AddOrderItemCommand>(request);
            await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Order item added successfully"
            });
        }
    }
}
