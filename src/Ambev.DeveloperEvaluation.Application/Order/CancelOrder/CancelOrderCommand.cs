using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Order.CancelOrder
{
    public class CancelOrderCommand : IRequest<CancelOrderResponse>
    {
        public Guid Id { get; }

        public CancelOrderCommand(Guid id)
        {
            Id = id;
        }
    }
}