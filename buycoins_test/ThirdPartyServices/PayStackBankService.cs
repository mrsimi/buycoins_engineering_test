using buycoins_test.ThirdPartyDTO.Responses; 
using Newtonsoft.Json;
using buycoins_test.DTO.Requests;
using System.Net;

namespace buycoins_test.ThirdPartyServices
{
    public class PayStackBankService
    {
        private readonly IConfiguration _configuration;
        private string PaystackBankResolveUrl;
        private string SecretKey; 
        public PayStackBankService(IConfiguration configuration)
        {
            _configuration = configuration;
            PaystackBankResolveUrl  = _configuration.GetSection("Paystack:BankResolveUrl").Value;
            SecretKey = _configuration.GetSection("Paystack:TestSecretKey").Value;
        }   
        public async Task<PaystackResolveBankInfoResponse> ResolveUserBankInfo(BankInfoRequest request)
        {
           try
           {
                string url = PaystackBankResolveUrl.Replace("{0}", request.AccountNumber).Replace("{1}", request.BankCode);

                HttpClient client = new HttpClient(); 
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {SecretKey}");

                var response = await client.GetAsync(url);

                // response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var responseJson = JsonConvert.DeserializeObject<PayStackResolveBankResponseRoot>(responseContent);

                return new PaystackResolveBankInfoResponse
                {
                    HttpStatusCode = (int)response.StatusCode, 
                    Response = responseJson
                };
           }
           catch(Exception ex)
           {
               return new PaystackResolveBankInfoResponse
               {
                   HttpStatusCode = (int)HttpStatusCode.InternalServerError, 
                   Response = new PayStackResolveBankResponseRoot
                   {
                       data = null, 
                       status = false, 
                       message = ex.Message
                   }
               };
           }
        }
    }
}