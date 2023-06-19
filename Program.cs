using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using PaymentGatewayCLI.services;


namespace PaymentGatewayCLI
{
    public class config
    {
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
        public string[] Currencies { get; set; }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            string json = File.ReadAllText("config.json");
            config config = JsonConvert.DeserializeObject<config>(json);
            Console.WriteLine($"APIkey: {config.ApiKey}");
            Console.WriteLine($"APIurl: {config.ApiUrl}");
            var apiService = new ApiService(config.ApiUrl, config.ApiKey);

            // Call the PostTransactionAsync method with the appropriate arguments
            // create a new TransactionRequest object with the necessary fields
            var request = new TransactionRequest
            {
                FirstName = "",
                LastName = "Doe",
                Address = "123 Main St",
                CustomerOrderId = "1234",
                Country = "US",
                State = "CA",
                City = "San Francisco",
                Zip = "94105",
                IpAddress = "192.168.0.1",
                Email = "teste@example.com",
                CountryCode = "US",
                PhoneNo = "1234567890",
                Amount = 123m,
                Currency = "USD",
                CardNo = "4000 0000 0000 3220",
                CcExpiryMonth = "12",
                CcExpiryYear = "2023",
                CvvNumber = "123",
                ResponseUrl = "https://example.com/response",
                WebhookUrl = "https://example.com/webhook"
            };

            // call the PostTransactionAsync method with the request object
            var response = await apiService.PostTransactionAsync(request);

            //Console.WriteLine($"Response status code: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                // Payment was successful
                PaymentResponse paymentResponse = PaymentResponse.ProcessPaymentResponse(response);
                Console.WriteLine($"Transaction was successful: {response.Content.ReadAsStringAsync().Result}");
                Console.WriteLine($"Payment ID: {paymentResponse.PaymentId}");
                Console.WriteLine($"Payment status: {paymentResponse.Status}");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.PaymentRequired)
            {
                // Payment requires 3DS authentication
                PaymentResponse paymentResponse = PaymentResponse.ProcessPaymentResponse(response);
                Console.WriteLine($"Payment requires 3DS authentication: {paymentResponse.ErrorMessage}");
                Console.WriteLine($"3DS redirect URL: {paymentResponse.ThreeDSRedirectUrl}");
            }
            else
            {
                // Payment failed
                PaymentResponse paymentResponse = PaymentResponse.ProcessPaymentResponse(response);
                Console.WriteLine($"Transaction failed: {paymentResponse.ErrorMessage}");
            }
        }

    }
}
