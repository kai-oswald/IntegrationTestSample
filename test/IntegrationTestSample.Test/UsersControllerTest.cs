using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using IntegrationTestSample.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace IntegrationTestSample.Test
{
    [TestClass]
    public class UsersControllerTest : IntegrationTestInitializer
    {
        [TestMethod]
        public async Task CanGetUsers()
        {
            List<string> expectedResponse = new List<string> { "Foo", "Bar", "Baz" };

            await PerformLogin("Test", "hunter2");

            var responseJson = await _client.GetStringAsync("api/users");
            List<string> actualResponse = JsonConvert.DeserializeObject<List<string>>(responseJson);

            CollectionAssert.AreEqual(expectedResponse, actualResponse);
        }

        [TestMethod]
        public async Task GetUsersUnauthorizedShouldReturn401()
        {
            var response = await _client.GetAsync("api/users");

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        private async Task PerformLogin(string userName, string password)
        {
            var user = new UserLoginModel
            {
                UserName = userName,
                Password = password
            };

            var res = await _client.PostAsJsonAsync("api/account/login", user);

            if (res.Headers.TryGetValues("Set-Cookie", out var cookies))
            {
                var authCookie = cookies.First();
                authCookie = authCookie.Replace("auth_cookie=", string.Empty);
            }
        }
    }
}
