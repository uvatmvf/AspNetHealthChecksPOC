using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AspNetHealthChecksPOC
{
    public class ReadinessPublisher : IHealthCheckPublisher
    {
        public Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
        {
            if (report.Status == HealthStatus.Healthy)
            {
                Console.WriteLine($"{DateTime.UtcNow} Readiness Probe Status: {report.Status}");
            }
            else
            {
                Console.WriteLine($"{DateTime.UtcNow} Readiness Probe Status: {report.Status}");
            }

            cancellationToken.ThrowIfCancellationRequested();

            return Task.CompletedTask;
        }
    }
}
