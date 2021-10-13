using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AspNetHealthChecksPOC
{
    public class HealthCheckWithArgs : IHealthCheck
    {
        private string _arg1;
        private int _arg2;

        public HealthCheckWithArgs(string arg1, int arg2)
        {
            _arg1 = arg1;
            _arg2 = arg2;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var random = new Random(DateTime.Now.Millisecond);
            var healthCheckResultHealthy = random.Next(0, 1) == 0;

            if (healthCheckResultHealthy)
            {
                return Task.FromResult(HealthCheckResult.Healthy($"{_arg1} is healthy as a horse"));
            }

            return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus,
                $"An unhealthy result- {_arg2}"));
        }
    }
}
