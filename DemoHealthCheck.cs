using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AspNetHealthChecksPOC
{
    public class DemoHealthCheck : IHealthCheck
    {
        private string _arg1;
        private int _arg2;
        private bool _lastCheckValue;

        public DemoHealthCheck(string arg1, int arg2)
        {
            _arg1 = arg1;
            _arg2 = arg2;
        }

        public DemoHealthCheck(string arg1)
        {
            _arg1 = arg1;
            _arg2 = -1;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var healthCheckResultHealthy = _lastCheckValue;
            _lastCheckValue = !_lastCheckValue;

            if (healthCheckResultHealthy)
            {
                return Task.FromResult(HealthCheckResult.Healthy($"{_arg1} is healthy as a horse"));
            }

            return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus,
                $"An unhealthy result-{_arg2}"));
        }
    }
}
