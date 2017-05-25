using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using System;

namespace StarterPack.Test
{
    public class SupportTest : BaseTest
    {
        [Fact]
        public async Task ShouldLoadLangs()
        {
            var response = await _client.GetAsync($"{_apiPrefix}/support/langs");
            response.EnsureSuccessStatusCode();

            var responseString = response.Content.ReadAsStringAsync().Result;
            dynamic jsonObject = JObject.Parse(responseString);

            Assert.NotNull(jsonObject.attributes);
        }
    }
}
