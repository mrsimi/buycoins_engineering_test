namespace buycoins_test.ThirdPartyDTO.Responses
{
    public class Data
    {
        public string account_number { get; set; }
        public string account_name { get; set; }
        public int bank_id { get; set; }
    }

    public class PaystackResolveBankInfoResponse
    {
      public int HttpStatusCode { get; set; }
      public PayStackResolveBankResponseRoot Response {get; set;}
    }

    public class PayStackResolveBankResponseRoot
    {
        public bool status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }
}