using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Order.ListOrders
{
    public class ListOrdersCommand : IRequest<List<ListOrdersResult>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }
    }
}
