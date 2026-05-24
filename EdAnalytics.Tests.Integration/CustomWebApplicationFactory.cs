using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using EdAnalytics.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EdAnalytics.Tests.Integration
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        public CustomWebApplicationFactory()
        {
            // Definir variáveis de ambiente ANTES de carregar o Program.cs do WebApplicationFactory
            Environment.SetEnvironmentVariable("UseInMemoryDatabase", "true");
            Environment.SetEnvironmentVariable("MongoDb__ConnectionString", "mongodb://localhost:27017");
            Environment.SetEnvironmentVariable("MongoDb__DatabaseName", "EdAnalyticsDbTest");
            Environment.SetEnvironmentVariable("Jwt__Key", "EdAnalyticsPremium2025SecretKeyMustBeAtLeast32Chars!!");
            Environment.SetEnvironmentVariable("Jwt__Issuer", "EdAnalytics.API");
            Environment.SetEnvironmentVariable("Jwt__Audience", "EdAnalytics.Client");
            Environment.SetEnvironmentVariable("Jwt__ExpirationMinutes", "60");
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Como o Program.cs já usará InMemory, não precisamos fazer substituição manual aqui.
            });
        }
    }
}
