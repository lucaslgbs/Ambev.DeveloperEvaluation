using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Order.ListOrders
{
    public class ListOrdersHandler : IRequestHandler<ListOrdersCommand, List<ListOrdersResult>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private ILogger<ListOrdersHandler> _logger;

        public ListOrdersHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<ListOrdersHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<ListOrdersResult>> Handle(ListOrdersCommand request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetPagedOrdersAsync(request.Page, request.PageSize, request.Search, cancellationToken);
            return _mapper.Map<List<ListOrdersResult>>(orders);
        }
    }
}
