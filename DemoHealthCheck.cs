using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AspNetHealthChecksPOC
{
    public class DemoHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var random = new Random(DateTime.Now.Millisecond);
            var healthCheckResultHealthy = random.Next(0,2) == 0 ;

            if (healthCheckResultHealthy)
            {
                return Task.FromResult(HealthCheckResult.Healthy($"is healthy as a horse"));
            }

            return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus,
                $"An unhealthy result"));
        }
    }
}
