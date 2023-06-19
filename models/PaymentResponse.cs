using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;

using Newtonsoft.Json.Linq;

using System.Threading.Tasks;

namespace PaymentGatewayCLI.services
{
    public class PaymentResponse
    {
        public bool Success { get; set; }
        public string PaymentId { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public string ThreeDSRedirectUrl { get; set; }  

        public static PaymentResponse ProcessPaymentResponse(HttpResponseMessage response)
        {
            PaymentResponse paymentResponse = new PaymentResponse();

            if (response.IsSuccessStatusCode)
            {
                JObject responseJson = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                if ((string)responseJson["status"] == "3d_redirect")
                {
                    // Payment requires 3D Secure authentication
                    paymentResponse.Success = false;
                    paymentResponse.Status = "3d_redirect";
                    paymentResponse.PaymentId = (string)responseJson["payment_id"];
                    paymentResponse.ErrorMessage = (string)responseJson["message"];
                    paymentResponse.ThreeDSRedirectUrl = (string)responseJson["redirect_3ds_url"];
                }
                else
                {
                    // Payment was successful
                    paymentResponse.Success = true;
                    paymentResponse.PaymentId = (string)responseJson["payment_id"];
                    paymentResponse.Status = (string)responseJson["status"];
                }
            }
            else
            {
                // Payment failed
                JObject responseJson = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                paymentResponse.Success = false;
                paymentResponse.ErrorMessage = (string)responseJson["error_message"];
            }

            return paymentResponse;
        }
    }
}