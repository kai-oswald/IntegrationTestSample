using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace IntegrationTestSample.Test
{
    [TestClass]
    public class ValuesControllerTest : IntegrationTestInitializer
    {
        [TestMethod]
        public async Task CanGetValues()
        {
            List<string> expectedResponse = new List<string> { "value1", "value2" };

            var responseJson = await _client.GetStringAsync("api/values");
            List<string> actualResponse = JsonConvert.DeserializeObject<List<string>>(responseJson);

            Assert.AreEqual(expectedResponse.Count, actualResponse.Count);
            foreach(var expectedValue in expectedResponse)
            {
                Assert.IsTrue(actualResponse.Contains(expectedValue));
            }
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(5)]
        [DataRow(99)]
        public async Task CanGetValue(int id)
        {
            string expectedResponse = "value";

            var responseContent = await _client.GetStringAsync($"api/values/{id}");

            Assert.AreEqual(expectedResponse, responseContent);
        }

        [TestMethod]
        [DataRow("foo")]
        [DataRow("bar")]
        [DataRow("Hello world")]
        public async Task CanCreateValue(string value)
        {
            string expectedResponse = value;

            var response = await _client.PostAsJsonAsync("api/values", value);
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(expectedResponse, responseContent);
        }

        [TestMethod]
        [DataRow(1, "foo")]
        [DataRow(5, "bar")]
        [DataRow(99, "Hello world")]
        public async Task CanUpdateValue(int id, string value)
        {
            string expectedResponse = value;

            var response = await _client.PutAsJsonAsync($"api/values/{id}", value);
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(expectedResponse, responseContent);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(5)]
        [DataRow(99)]
        public async Task CanDeleteValue(int id)
        {
            var response = await _client.DeleteAsync($"api/values/{id}");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

    }
}
