using System;
using System.Collections.Generic;

namespace Challenge.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string FirstName{ get; private set; }
        public string LastName { get; private set; }
        public virtual ICollection<Order> Orders { get; private set; }

        public void ChangeCustomerName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
