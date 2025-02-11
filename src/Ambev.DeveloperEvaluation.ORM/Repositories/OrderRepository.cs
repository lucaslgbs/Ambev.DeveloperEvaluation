using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DefaultContext _context;

        public OrderRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _context.Orders.AddAsync(order, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return order;
        }

        public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Orders.Include(x => x.Items).FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task CreateAsync(OrderItem item, CancellationToken cancellationToken = default)
        {
            await _context.AddAsync(item, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(OrderItem item, CancellationToken cancellationToken = default)
        {
            await _context.AddAsync(item, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Order>> GetPagedOrdersAsync(int page, int pageSize, string? search, CancellationToken cancellationToken)
        {
            var query = _context.Orders.Include(x => x.Items).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(o => o.OrderNumber.Contains(search) ||
                                         o.Customer.Contains(search) ||
                                         o.Branch.Contains(search));
            }

            return await query.Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync(cancellationToken);
        }
    }
}
