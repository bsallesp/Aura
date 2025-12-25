using Aesthetic.Application.Authentication.Commands.Register;
using Aesthetic.Application.Services.Commands.CreateService;
using Aesthetic.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Aesthetic.IntegrationTests.Scenarios;

public class ProfessionalFlowTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ProfessionalFlowTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task RegisterAndCreateService_ShouldSucceed()
    {
        // 1. Register a new Professional
        var registerCommand = new RegisterCommand(
            "John",
            "Doe",
            $"john.doe.{Guid.NewGuid()}@example.com",
            "Password123!",
            "Professional",
            "John's Aesthetic Clinic"
        );

        var registerResponse = await _client.PostAsJsonAsync("/auth/register", registerCommand);
        registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var authResult = await registerResponse.Content.ReadFromJsonAsync<AuthResponse>();
        authResult.Should().NotBeNull();
        authResult!.Token.Should().NotBeNullOrEmpty();

        // 2. Set Authorization Header
        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResult.Token);

        // 3. Create a Service
        var createServiceRequest = new CreateServiceRequest(
            "Botox Treatment",
            150.00m,
            45,
            "Anti-aging treatment"
        );
        
        var createServiceResponse = await _client.PostAsJsonAsync("/services", createServiceRequest);
        
        if (createServiceResponse.StatusCode != HttpStatusCode.Created)
        {
            var error = await createServiceResponse.Content.ReadAsStringAsync();
            // Force failure with error message
            createServiceResponse.StatusCode.Should().Be(HttpStatusCode.Created, $"Error response: {error}");
        }
        
        createServiceResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var serviceResponse = await createServiceResponse.Content.ReadFromJsonAsync<ServiceResponse>();
        serviceResponse.Should().NotBeNull();
        serviceResponse!.Id.Should().NotBeEmpty();
    }

    // Helper records
    private record AuthResponse(string Token, string RefreshToken);
    private record CreateServiceRequest(string Name, decimal Price, int DurationMinutes, string? Description);
    private record ServiceResponse(Guid Id, Guid ProfessionalId, string Name, decimal Price, int DurationMinutes, string? Description, bool IsActive);
}
