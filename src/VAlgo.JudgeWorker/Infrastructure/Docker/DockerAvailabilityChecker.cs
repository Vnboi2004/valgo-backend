using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace VAlgo.JudgeWorker.Infrastructure.Docker
{
    public sealed class DockerAvailabilityChecker
    {
        private readonly ILogger<DockerAvailabilityChecker> _logger;

        public DockerAvailabilityChecker(ILogger<DockerAvailabilityChecker> logger)
        {
            _logger = logger;
        }

        public async Task EnsureAvailableAsync()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "docker",
                    Arguments = "info",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };

            process.Start();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                _logger.LogCritical("Docker is not available");
                throw new InvalidOperationException("Docker deamon is not available");
            }

            _logger.LogInformation("Docker is available");
        }
    }
}