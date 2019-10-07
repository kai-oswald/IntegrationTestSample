using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IntegrationTestSample.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace IntegrationTestSample.Test
{
    [TestClass]
    public class UsersControllerTest : IntegrationTestInitializer
    {
        #region Cookie
        [TestMethod]
        public async Task CanGetUsers()
        {
            List<string> expectedResponse = new List<string> { "Foo", "Bar", "Baz" };

            await PerformLogin("Test", "hunter2");

            var responseJson = await _client.GetStringAsync("api/users");
            List<string> actualResponse = JsonConvert.DeserializeObject<List<string>>(responseJson);

            CollectionAssert.AreEqual(expectedResponse, actualResponse);
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

        #endregion

        #region Jwt
        [TestMethod]
        public async Task CanGetUsersJwt()
        {
            var token = await GetToken("foo", "bar");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("api/users");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task GetUsersJwtInvalidTokenShouldReturnUnauthorized()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "invalid_token");

            var response = await _client.GetAsync("api/users");
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        private async Task<string> GetToken(string userName, string password)
        {
            var user = new UserLoginModel
            {
                UserName = userName,
                Password = password
            };

            var res = await _client.PostAsJsonAsync("api/account/token", user);

            if(!res.IsSuccessStatusCode) return null;

            var userModel = await res.Content.ReadAsAsync<User>();

            return userModel?.Token;
        }
        #endregion

        [TestMethod]
        public async Task GetUsersUnauthorizedShouldReturn401()
        {
            var response = await _client.GetAsync("api/users");

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

    }
}
