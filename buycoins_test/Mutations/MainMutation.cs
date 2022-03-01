using buycoins_test.DTO.Responses;
using buycoins_test.DTO.Requests;
using buycoins_test.ThirdPartyServices;
using System.Net;
using System.Globalization;
using buycoins_test.Models;
using Quickenshtein;

namespace buycoins_test.Mutations
{
    public class MainMutation
    {
        private readonly IConfiguration _configuration;
        private PayStackBankService _paystackService;
        private readonly DataContext _dbContext;
      

        public MainMutation(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbContext = new DataContext();

            _paystackService = new PayStackBankService(_configuration);
        }
        public async Task<BankInfoResponse> VerifyBankInformation(BankInfoRequest request)
        {
            
            var res = await _paystackService.ResolveUserBankInfo(request);

            if(res.HttpStatusCode == (int)HttpStatusCode.OK && res.Response.status == true)
            {
                var distance = Levenshtein.GetDistance(request.AccountName.ToLower(), 
                    res.Response.data.account_name.ToLower());

                if(distance > 2)
                {
                    return new BankInfoResponse
                    {
                        StatusCode = (int)HttpStatusCode.Conflict,
                        AccountName = string.Empty,
                        ResponseMessage = "Disparity in account name. Input Name differs from name " +
                        "Registered, kindly check your account name and try again"
                    };
                }
                string accountName = new CultureInfo("en-US", false).TextInfo.ToTitleCase(res.Response.data.account_name.ToLower());

                var newBankUserInfo = new UserBankInfo
                {
                    AccountName = accountName,
                    AccountNumber = request.AccountNumber, 
                    BankCode = request.BankCode, 
                    IsVerified  = true,
                };

                await _dbContext.UserBankInfos.AddAsync(newBankUserInfo);
                await _dbContext.SaveChangesAsync();


                return new BankInfoResponse
                {
                    StatusCode = res.HttpStatusCode, 
                    AccountName = accountName, 
                    ResponseMessage = res.Response.message
                };
            }

            return new BankInfoResponse
            {
                StatusCode = res.HttpStatusCode, 
                AccountName = string.Empty, 
                ResponseMessage = res.Response.message
            };
        }



    }
}