using Challenge.Entities;
using Serilog;
using System.Data.Entity;
using System.Diagnostics;

namespace Challenge.Infrastructure
{
    public class OrdersContext : DbContext
    {
        public DbSet<Customer> Customer { get; set; }

        public DbSet<Order> Order { get; set; }

        private readonly ILogger _logger;

        public OrdersContext(ILogger logger)
        {
            _logger = logger;
            Database.Log = s => _logger?.Debug(s);

            //disable initializer
            Database.SetInitializer<OrdersContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Customer>().HasMany(m => m.Orders).WithRequired(r => r.Customer).Map(m => m.MapKey("CustomerId"));

            modelBuilder.Entity<Order>().ToTable("Order");
        }
    }
}
