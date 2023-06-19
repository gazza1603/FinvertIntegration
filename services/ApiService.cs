using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;


namespace PaymentGatewayCLI.services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly string _apiKey;

        public ApiService(string apiUrl, string apiKey)
        {
            _apiUrl = apiUrl;
            _apiKey = apiKey;
            _httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> PostTransactionAsync(TransactionRequest request)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            { 
                { "api_key", _apiKey },
                { "first_name", request.FirstName },
                { "last_name", request.LastName },
                { "address", request.Address },
                { "customer_order_id", request.CustomerOrderId },
                { "country", request.Country },
                { "state", request.State },
                { "city", request.City },
                { "zip", request.Zip },
                { "ip_address", request.IpAddress },
                { "email", request.Email },
                { "country_code", request.CountryCode },
                { "phone_no", request.PhoneNo },
                { "amount", request.Amount.ToString("0.00") },
                { "currency", request.Currency },
                { "card_no", request.CardNo },
                { "ccExpiryMonth", request.CcExpiryMonth },
                { "ccExpiryYear", request.CcExpiryYear },
                { "cvvNumber", request.CvvNumber },
                { "response_url", request.ResponseUrl },
                { "webhook_url", request.WebhookUrl }
            });
            var response = await _httpClient.PostAsync(_apiUrl, content);

            if (response.StatusCode == HttpStatusCode.Found)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var paReq = GetParameterValue(responseContent, "PaReq");
                var termUrl = GetParameterValue(responseContent, "TermUrl");
                var md = GetParameterValue(responseContent, "MD");

                // redirect the user to the 3DS authentication page
                var authenticationUrl = "https://paymentgateway.com/3dsauthentication";
                var redirectUrl = $"{authenticationUrl}?PaReq={paReq}&TermUrl={termUrl}&MD={md}";
                //Console.WriteLine(responseContent);
                return new HttpResponseMessage(HttpStatusCode.Redirect) { Headers = { Location = new Uri(redirectUrl) } };
            }

            return response;


            //return await _httpClient.PostAsync(_apiUrl, content);
        }
        private static string GetParameterValue(string responseContent, string parameterName)
        {
            var parameterStartIndex = responseContent.IndexOf($"{parameterName}=", StringComparison.Ordinal);
            if (parameterStartIndex == -1)
            {
                return null;
            }

            parameterStartIndex += parameterName.Length + 1;
            var parameterEndIndex = responseContent.IndexOf('&', parameterStartIndex);
            if (parameterEndIndex == -1)
            {
                parameterEndIndex = responseContent.Length;
            }

            return responseContent.Substring(parameterStartIndex, parameterEndIndex - parameterStartIndex);
        }
    }
}