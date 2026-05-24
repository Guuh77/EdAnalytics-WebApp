using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EdAnalytics.Application.DTOs;
using Xunit;

namespace EdAnalytics.Tests.Integration
{
    public class CursosControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CursosControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ReturnsPaginatedCursosAndHateoasLinks()
        {
            // Act
            var response = await _client.GetAsync("/api/cursos?page=1&pageSize=2");
            var contentString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK, $"Status was {response.StatusCode}. Response content: {contentString}");
            
            // O response contem items, totalCount, etc.
            Assert.Contains("items", contentString);
            Assert.Contains("totalCount", contentString);
            Assert.Contains("totalPages", contentString);
            Assert.Contains("links", contentString); // HATEOAS links
            Assert.Contains("self", contentString);
        }

        [Fact]
        public async Task CreateCurso_WithoutAuth_ReturnsUnauthorized()
        {
            // Arrange
            var novoCurso = new
            {
                Titulo = "Curso Sem Autenticação",
                Area = "Outros",
                Visualizacoes = 0
            };
            var content = new StringContent(JsonSerializer.Serialize(novoCurso), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/cursos", content);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CreateCurso_WithAdminAuth_ReturnsCreated()
        {
            // Arrange - Realizar login
            var credentials = new LoginDto
            {
                Email = "admin@edanalytics.com",
                Password = "Admin@123"
            };
            var loginContent = new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, "application/json");
            var loginResponse = await _client.PostAsync("/api/auth/login", loginContent);
            loginResponse.EnsureSuccessStatusCode();

            var loginResult = JsonSerializer.Deserialize<TokenResponseDto>(
                await loginResponse.Content.ReadAsStringAsync(), 
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            
            Assert.NotNull(loginResult);
            
            // Configurar token no header
            var clientWithAuth = _client;
            clientWithAuth.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.Token);

            var novoCurso = new
            {
                Titulo = "Curso Com Autenticação",
                Area = "Tecnologia",
                Visualizacoes = 10
            };
            var content = new StringContent(JsonSerializer.Serialize(novoCurso), Encoding.UTF8, "application/json");

            // Act
            var response = await clientWithAuth.PostAsync("/api/cursos", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
