using Aesthetic.Application.Common.Interfaces.Authentication;
using Aesthetic.Application.Common.Interfaces.Payments;
using Aesthetic.Domain.Interfaces;
using Aesthetic.Infrastructure.Authentication;
using Aesthetic.Infrastructure.Payments;
using Aesthetic.Infrastructure.Persistence;
using Aesthetic.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Stripe;

namespace Aesthetic.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AestheticDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            services.AddSingleton<ITokenGenerator, JwtTokenGenerator>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            services.Configure<StripeSettings>(configuration.GetSection(StripeSettings.SectionName));
            services.AddScoped<IPaymentService, StripePaymentService>();

            services.AddResiliencePipeline("stripe-pipeline", builder =>
            {
                builder
                    .AddRetry(new Polly.Retry.RetryStrategyOptions
                    {
                        ShouldHandle = new PredicateBuilder().Handle<StripeException>(),
                        Delay = TimeSpan.FromSeconds(2),
                        MaxRetryAttempts = 3,
                        BackoffType = DelayBackoffType.Exponential,
                        UseJitter = true
                    })
                    .AddCircuitBreaker(new Polly.CircuitBreaker.CircuitBreakerStrategyOptions
                    {
                        ShouldHandle = new PredicateBuilder().Handle<StripeException>(),
                        SamplingDuration = TimeSpan.FromSeconds(30),
                        FailureRatio = 0.5,
                        MinimumThroughput = 5,
                        BreakDuration = TimeSpan.FromSeconds(30)
                    })
                    .AddTimeout(TimeSpan.FromSeconds(10));
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProfessionalRepository, ProfessionalRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}