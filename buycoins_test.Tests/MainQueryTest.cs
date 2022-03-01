
using buycoins_test.Queries;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Snapshooter.Xunit;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace buycoins_test.Tests
{
    public class MainQueryTest
    {
       
        [Fact]
        public async Task GetUserAccountName_ReturnsOk()
        {
            var serviceCollection = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile(
                        path: "appsettings.json",
                        optional: false,
                        reloadOnChange: true)
                  .Build();
            serviceCollection.AddSingleton<IConfiguration>(configuration);

            var executor = await serviceCollection
                    .AddGraphQLServer()
                    .AddQueryType<MainQuery>()
                    .BuildRequestExecutorAsync();

            var query = QueryRequestBuilder.New()
                    .SetQuery("query{ userAccountName(accountNumber: \"0161023149\", bankCode: \"058\") { accountName statusCode } }")
                    .Create();


            var result = await executor.ExecuteAsync(query);
            Assert.Null(result.Errors);

            var jsonResult = JObject.Parse(result.ToJson());

            var accountName = jsonResult["data"]["userAccountName"]["accountName"]?.ToString();
            int statusCode = 0;
            int.TryParse(jsonResult["data"]["userAccountName"]["statusCode"]?.ToString(), out statusCode);
            
            Assert.Equal(200, statusCode);
            result.MatchSnapshot();
        }
    }
}