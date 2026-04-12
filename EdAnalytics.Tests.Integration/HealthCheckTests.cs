using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EdAnalytics.Tests.Integration
{
    public class HealthCheckTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public HealthCheckTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetHealthCheckEndpoint_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/health");

            // Assert
            response.EnsureSuccessStatusCode(); // Retorna sucess se status for 200-299
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
            
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("status", responseString);
        }

        [Fact]
        public async Task GetIndexPage_ReturnsOkAndContainsEdAnalytics()
        {
            // Act
            var response = await _client.GetAsync("/");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            
            // O título original no _Layout pode conter "EdAnalytics"
            Assert.Contains("EdAnalytics", responseString);
        }
    }
}
