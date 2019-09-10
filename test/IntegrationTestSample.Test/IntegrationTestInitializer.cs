using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;

namespace IntegrationTestSample.Test
{
    [TestClass]
    public abstract class IntegrationTestInitializer
    {
        protected HttpClient _client;

        [TestInitialize]
        public void Setup()
        {
            var builder = new WebHostBuilder()
               .UseStartup<Startup>();
            var server = new TestServer(builder);

            _client = server.CreateClient();
        }
    }
}
