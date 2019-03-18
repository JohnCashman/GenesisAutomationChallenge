using Challenge.Entities;
using Challenge.Infrastructure;
using MediatR;
using Serilog;
using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace Challenge.Queries
{
    public static class CustomerQueries
    {
        public class CustomerQuery : IRequest<Customer>
        {
            public Guid CustomerId { get; set; }

            public CustomerQuery(Guid customerId)
            {
                CustomerId = customerId;
            }
        }

        public class Handler : IRequestHandler<CustomerQuery, Customer>
        {
            private readonly OrdersContext _context;
            private readonly ILogger _logger;

            public Handler(OrdersContext context, ILogger logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<Customer> Handle(CustomerQuery request, CancellationToken cancellationToken)
            {
                _logger?.ForContext<OrderSumaryQueries>().Debug("Retrieving customer");

                return await _context.Customer.FirstOrDefaultAsync(f => f.Id.Equals(request.CustomerId));
            }
        }
    }
}
