using buycoins_test.Mutations;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Snapshooter.Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace buycoins_test.Tests
{
    public class MainMutationTest
    {

        [Fact]
        public async Task VerifyBankInfo_ReturnOkResult()
        {
           
            var executor = await new ServiceCollection()
                   .AddGraphQLServer()
                   .ConfigureSchema(sb => sb.ModifyOptions(opts => opts.StrictValidation = false))
                   .AddMutationType<MainMutation>()
                   .BuildRequestExecutorAsync();

            var query = QueryRequestBuilder.New()
                   .SetQuery("mutation{ verifyBankInformation(request: { accountName: \"Adegoke Similoluwa Matthiass\", " +
                   "accountNumber: \"0161023149\", bankCode: \"058\"}) {accountName statusCode }}")
                   .Create();

            var result = await executor.ExecuteAsync(query);
            
            
            //assert
            Assert.Null(result.Errors);
            var jsonResult = JObject.Parse(result.ToJson());

            var accountName = jsonResult["data"]["verifyBankInformation"]["accountName"]?.ToString();
            int statusCode = 0; 
            int.TryParse(jsonResult["data"]["verifyBankInformation"]["statusCode"]?.ToString(), out statusCode);


            Assert.NotNull(accountName);
            Assert.Equal(200, statusCode);
            result.MatchSnapshot();
        }

        [Fact]
        public async Task VerifyBankInfo_WithLDGreaterThan2_Return409()
        {
            var executor = await new ServiceCollection()
                   .AddGraphQLServer()
                   .ConfigureSchema(sb => sb.ModifyOptions(opts => opts.StrictValidation = false))
                   .AddMutationType<MainMutation>()
                   .BuildRequestExecutorAsync();

            var query = QueryRequestBuilder.New()
                   .SetQuery("mutation{ verifyBankInformation(request: { accountName: \"John Doe\", " +
                   "accountNumber: \"0161023149\", bankCode: \"058\"}) {accountName statusCode}}")
                   .Create();

            var result = await executor.ExecuteAsync(query);


            //assert
            Assert.Null(result.Errors);
            var jsonResult = JObject.Parse(result.ToJson());

            var accountName = jsonResult["data"]["verifyBankInformation"]["accountName"]?.ToString();
            int statusCode = 0;
            int.TryParse(jsonResult["data"]["verifyBankInformation"]["statusCode"]?.ToString(), out statusCode);


            Assert.NotNull(accountName);
            Assert.Equal(409, statusCode);
            result.MatchSnapshot();
        }

        [Fact]
        public async Task VerifyBankInfo_InvalidAccountNumber_Return422()
        {
           
            var executor = await new ServiceCollection()
                   .AddGraphQLServer()
                   .ConfigureSchema(sb => sb.ModifyOptions(opts => opts.StrictValidation = false))
                   .AddMutationType<MainMutation>()
                   .BuildRequestExecutorAsync();

            var query = QueryRequestBuilder.New()
                   .SetQuery("mutation{ verifyBankInformation(request: { accountName: \"John Doe\", " +
                   "accountNumber: \"01610231490909\", bankCode: \"058\"}) {accountName statusCode}}")
                   .Create();

            var result = await executor.ExecuteAsync(query);


            //assert
            Assert.Null(result.Errors);
            var jsonResult = JObject.Parse(result.ToJson());

            var accountName = jsonResult["data"]["verifyBankInformation"]["accountName"]?.ToString();
            int statusCode = 0;
            int.TryParse(jsonResult["data"]["verifyBankInformation"]["statusCode"]?.ToString(), out statusCode);


            Assert.NotNull(accountName);
            Assert.Equal(422, statusCode);
            result.MatchSnapshot();
        }
    }
}
