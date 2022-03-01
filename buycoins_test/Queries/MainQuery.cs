using buycoins_test.DTO.Requests;
using buycoins_test.DTO.Responses;
using buycoins_test.Models;
using buycoins_test.ThirdPartyServices;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net;

namespace buycoins_test.Queries
{
    public class MainQuery
    {
        private readonly IConfiguration _configuration;
        private PayStackBankService _paystackService;
        private readonly DataContext _context;
        public MainQuery(IConfiguration configuration)
        {
            _context = new DataContext();
            _configuration = configuration;

           
            _paystackService = new PayStackBankService(_configuration);
        }

        public BankInfoResponse GetUserBankInfo() =>
            new BankInfoResponse
            {
                AccountName = "Similoluwa"
            };

        public string SayHello() => "Hellow";
       
        public async Task<BankInfoResponse> GetUserAccountName(string accountNumber, string bankCode)
        {
            try
            {
                var userBankInfo = _context.UserBankInfos
                        .Where(m => m.AccountNumber.Equals(accountNumber) && m.BankCode.Equals(bankCode)).FirstOrDefault();

                if (userBankInfo == null)
                {
                    var request = new BankInfoRequest { AccountNumber = accountNumber, BankCode = bankCode };
                    var res = await _paystackService.ResolveUserBankInfo(request);

                    string accountName = new CultureInfo("en-US", false).TextInfo.ToTitleCase(res.Response.data.account_name.ToLower());

                    var newBankUserInfo = new UserBankInfo
                    {
                        AccountName = accountName,
                        AccountNumber = request.AccountNumber,
                        BankCode = request.BankCode,
                        IsVerified = true,
                    };

                    await _context.UserBankInfos.AddAsync(newBankUserInfo);
                    await _context.SaveChangesAsync();


                    return new BankInfoResponse
                    {
                        StatusCode = res.HttpStatusCode,
                        AccountName = accountName,
                        ResponseMessage = res.Response.message
                    };
                }

                else
                {
                    return new BankInfoResponse
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        AccountName = userBankInfo.AccountName,
                        ResponseMessage = "Data request successful"
                    };
                }
            }
            catch (Exception ex)
            {
                return new BankInfoResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    AccountName = String.Empty,
                    ResponseMessage = ex.Message
                };
            }
        }
    }
}