using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Pokedex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class TestServerContainer : IDisposable, IAsyncLifetime
    {
        public TestServer Server { get; set; }

        public int Port { get; set; } = 27206;

        public void StartServer()
        {
            var builder = BuildServer();
            CreateServer(builder);
        }

        public IWebHostBuilder BuildServer()
        {
            return Program.BuildWebHost(Array.Empty<string>());
        }

        public void CreateServer(IWebHostBuilder builder)
        {
            Server = new TestServer(builder)
            {
                BaseAddress = new Uri($"https://localhost:{Port}")
            };
        }

        public void Dispose()
        {
            Server?.Dispose();
        }

        public async Task InitializeAsync()
        {
            StartServer();
        }

        public async Task DisposeAsync()
        {
            await Task.Run(Dispose);
        }
    }

    [CollectionDefinition("Shared Server Collection")]
    public class SharedServerCollection : ICollectionFixture<TestServerContainer>
    {

    }
}
