namespace Ambev.DeveloperEvaluation.WebApi.Features.Order.ListOrders
{
    public class ListOrdersRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }
    }
}
