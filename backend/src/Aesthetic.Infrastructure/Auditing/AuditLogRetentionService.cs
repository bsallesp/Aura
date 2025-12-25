using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Aesthetic.Domain.Interfaces;

namespace Aesthetic.Infrastructure.Auditing;

public class AuditLogRetentionService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AuditLogRetentionService> _logger;
    private readonly int _retentionDays;

    public AuditLogRetentionService(IServiceProvider serviceProvider, ILogger<AuditLogRetentionService> logger, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _retentionDays = configuration.GetValue<int>("AuditLogRetentionDays", 365);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Audit Log Retention Service starting. Retention days: {Days}", _retentionDays);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var repository = scope.ServiceProvider.GetRequiredService<IAuditLogRepository>();
                    var cutoffDate = DateTime.UtcNow.AddDays(-_retentionDays);
                    
                    _logger.LogInformation("Deleting audit logs older than {CutoffDate}...", cutoffDate);
                    var deletedCount = await repository.DeleteOlderThanAsync(cutoffDate);
                    _logger.LogInformation("Deleted {Count} old audit logs.", deletedCount);
                }

                // Run once per day
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting old audit logs.");
                // Wait a bit before retrying if error occurs, e.g. 1 hour
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}