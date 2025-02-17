using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Order.GetOrder
{
    public class GetOrderHandler : IRequestHandler<GetOrderCommand, GetOrderResult>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrderHandler(
            IOrderRepository orderRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<GetOrderResult> Handle(GetOrderCommand request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request, cancellationToken);

            var order = await _orderRepository.GetByIdAsync(request.Id, cancellationToken);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {request.Id} not found");

            return _mapper.Map<GetOrderResult>(order);
        }

        private async Task ValidateRequest(GetOrderCommand request, CancellationToken cancellationToken)
        {
            var validator = new GetOrderValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }
    }
}
