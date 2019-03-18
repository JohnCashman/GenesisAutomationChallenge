using Challenge.Infrastructure;
using CSharpFunctionalExtensions;
using MediatR;
using Serilog;
using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace Challenge.Commands
{
    public static class CustomerUpdates
    {
        public class Command : IRequest<Result>
        {
            public Guid CustomerId { get; }
            public string FirstName { get; }
            public string LastName { get; }

            public Command(Guid CustomerId, string firstName, string lastName)
            {
                this.CustomerId = CustomerId;
                FirstName = firstName;
                LastName = lastName;
            }
        }

        public class CommandHandler : IRequestHandler<Command, Result>
        {
            private readonly OrdersContext _orderContext;
            private readonly ILogger _logger;

            public CommandHandler(OrdersContext orderContext, ILogger logger)
            {
                _orderContext = orderContext;
                _logger = logger;
            }
            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.CustomerId == Guid.Empty)
                {
                    throw new ArgumentNullException(nameof(request.CustomerId));
                }

                //Implementing a Second level cache would reduce DB trips.
                var customer = await _orderContext.Customer.FirstOrDefaultAsync(f => f.Id.Equals(request.CustomerId));

                customer.ChangeCustomerName(request.FirstName, request.LastName);

                try
                {
                    await _orderContext.SaveChangesAsync(cancellationToken);
                    _logger?.Debug($"Customer Saved - {request.CustomerId}");
                }
                catch (Exception ex)
                {
                    _logger?.Error(ex, $"Error saving customer - {request.CustomerId}");
                    return Result.Fail("Error saving customer");
                }

                return Result.Ok();
            }
        }
    }
}
