using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using System;

namespace StarterPack.Test
{
    public class BaseTest
    {
        protected readonly TestServer _server;
        protected readonly HttpClient _client;
        protected readonly string _apiPrefix;

        public BaseTest()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();
            _apiPrefix = "/api/v1";
        }
    }
}
