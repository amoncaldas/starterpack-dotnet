using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using System;

namespace StarterPack.Test
{
    public class SupportTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public SupportTest()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task ShouldLoadLangs()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/support/langs");
            response.EnsureSuccessStatusCode();

            var responseString = response.Content.ReadAsStringAsync().Result;
            dynamic jsonObject = JObject.Parse(responseString);

            Assert.NotNull(jsonObject.attributes);
        }
    }
}
