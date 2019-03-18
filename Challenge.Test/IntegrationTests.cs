using System;
using System.Threading;
using System.Threading.Tasks;
using Challenge.Commands;
using Challenge.Infrastructure;
using Challenge.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Challenge.Test
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public async Task TestSaveCustomer()
        {
            var orderContext = new OrdersContext(null);

            var newCustomerCommand = new CustomerUpdates.Command(Guid.Parse("c17a59df-0c7d-4f61-9bb9-e467861984a6"), "Alice", "Doyle1");
            var commandHandler = new CustomerUpdates.CommandHandler(orderContext, null);

            var result = await commandHandler.Handle(newCustomerCommand, CancellationToken.None).ConfigureAwait(false);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task TestMissingCustomerIDException()
        {
            var orderContext = new OrdersContext(null);

            var newCustomerCommand = new CustomerUpdates.Command(Guid.Empty, "Alice", "Doyle1");
            var commandHandler = new CustomerUpdates.CommandHandler(orderContext, null);

            try
            {
                var result = await commandHandler.Handle(newCustomerCommand, CancellationToken.None).ConfigureAwait(false);
                Assert.Fail("No Exception thrown");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentNullException);
            }
        }


        [TestMethod]
        public async Task TestLoadCustomer()
        {
            var orderContext = new OrdersContext(null);

            var customerQuery = new CustomerQueries.CustomerQuery(Guid.Parse("c17a59df-0c7d-4f61-9bb9-e467861984a6"));
            var queryHandler = new CustomerQueries.Handler(orderContext, null);

            var result = await queryHandler.Handle(customerQuery, CancellationToken.None);

            Assert.IsNotNull(result);
        }
    }
}
