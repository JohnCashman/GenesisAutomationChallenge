using AutoMapper;
using AutoMapper.QueryableExtensions;
using Challenge.DTOs;
using Challenge.Infrastructure;
using MediatR;
using Serilog;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace Challenge.Queries
{
    public class OrderSumaryQueries
    {
        public class OrderSummaryQuery : IRequest<IReadOnlyList<OrderSummaryDTO>>
        {
        }

        public class Handler : IRequestHandler<OrderSummaryQuery, IReadOnlyList<OrderSummaryDTO>>
        {
            private readonly IMapper _mapper;
            private readonly OrdersContext _context;
            private readonly ILogger _logger;

            public Handler(OrdersContext context, IMapper mapper, ILogger logger)
            {
                _mapper = mapper;
                _context = context;
                _logger = logger;
            }

            public async Task<IReadOnlyList<OrderSummaryDTO>> Handle(OrderSummaryQuery request, CancellationToken cancellationToken)
            {
                _logger.ForContext<OrderSumaryQueries>().Debug("Retrieving order summary");

                return await _context.Order.Include(i => i.Customer).ProjectTo<OrderSummaryDTO>(_mapper.ConfigurationProvider).AsNoTracking().ToListAsync(cancellationToken);
            }
        }
    }
}
