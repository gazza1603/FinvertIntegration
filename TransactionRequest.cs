using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGatewayCLI
{
    public class TransactionRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string CustomerOrderId { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string IpAddress { get; set; }
        public string Email { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNo { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string CardNo { get; set; }
        public string CcExpiryMonth { get; set; }
        public string CcExpiryYear { get; set; }
        public string CvvNumber { get; set; }
        public string ResponseUrl { get; set; }
        public string WebhookUrl { get; set; }
    }
}
