using System;

namespace Challenge.DTOs
{
    public class OrderSummaryDTO
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string ReferenceNumber { get; set; }
        public decimal OrderValue { get; set; }
        public DateTime OrderDate { get; set; }
        public string FullName { get; set; }

        public string OrderValueFormatted { get { return OrderValue.ToString("C"); } }
        public string OrderDateFormatted { get { return OrderDate.ToString("dd-MMM-yyyy"); } }
    }
}
