using Aesthetic.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;
using Xunit;

namespace Aesthetic.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .WithDatabase("aesthetic_test_db")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<AestheticDbContext>));

            services.AddDbContext<AestheticDbContext>(options =>
                options.UseNpgsql(_dbContainer.GetConnectionString())
                       .ConfigureWarnings(warnings => warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AestheticDbContext>();
        await context.Database.MigrateAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}
