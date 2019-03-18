using System;

namespace Challenge.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }
        public virtual Customer Customer { get; private set; }
        public string ReferenceNumber { get; private set; }
        public decimal OrderValue { get; private set; }
        public DateTime OrderDate { get; private set; }
    }
}
