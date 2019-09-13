using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;

namespace IntegrationTestSample.Test
{
    [TestClass]
    public abstract class IntegrationTestInitializer : WebApplicationFactory<Startup>
    {
        protected HttpClient _client;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing")
               .UseStartup<Startup>();
            base.ConfigureWebHost(builder);
        }

        [TestInitialize]
        public void Setup()
        {
            var builder = new WebHostBuilder()
                .UseEnvironment("Testing")
               .UseStartup<Startup>();

            _client = this.CreateClient();
        }
    }
}
