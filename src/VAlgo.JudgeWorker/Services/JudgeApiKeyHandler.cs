using Microsoft.Extensions.Configuration;

namespace VAlgo.JudgeWorker.Services
{
    public sealed class JudgeApiKeyHandler : DelegatingHandler
    {
        private readonly string _apiKey;
        private const string ApiKeyHeader = "X-Judge-Api-Key";

        public JudgeApiKeyHandler(IConfiguration configuration)
        {
            _apiKey = configuration["VAlgoApi:ApiKey"]
                ?? throw new InvalidOperationException("VAlgoApi:ApiKey is not configured.");
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            request.Headers.Add(ApiKeyHeader, _apiKey);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}