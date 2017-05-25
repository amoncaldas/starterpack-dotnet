using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using System;
using System.Text;
using Newtonsoft.Json;
using StarterPack.Models;

namespace StarterPack.Test
{
    public class LoginTest : BaseTest
    {
        [Fact]
        public async System.Threading.Tasks.Task ShouldLoginWithValidCredentials()
        {
            Login login = new Login();
            login.Email = "admin-base@prodeb.com";
            login.Password = "Prodeb01";

            var requestContent = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync($"{_apiPrefix}/authenticate", requestContent);

            response.EnsureSuccessStatusCode();

            var responseString = response.Content.ReadAsStringAsync().Result;
            dynamic jsonObject = JObject.Parse(responseString);

            Assert.NotNull(jsonObject.token);
        }
    }
}
