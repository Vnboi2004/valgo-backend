namespace VAlgo.API.Middleware
{
    public sealed class JudgeApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _expectedApiKey;
        private const string ApiKeyHeader = "X-Judge-Api-Key";

        public JudgeApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _expectedApiKey = configuration["JudgeWorker:ApiKey"]
                ?? throw new InvalidOperationException("JudgeWorker:ApiKey is not configured.");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (IsJudgeEndpoint(context.Request))
            {
                var apiKey = context.Request.Headers[ApiKeyHeader].FirstOrDefault();

                if (string.IsNullOrWhiteSpace(apiKey) || apiKey != _expectedApiKey)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new
                    {
                        error = "unauthorized",
                        message = "Invalid or missing Judge API key."
                    });
                    return;
                }
            }

            await _next(context);
        }

        private static bool IsJudgeEndpoint(HttpRequest request)
        {
            var path = request.Path.Value ?? string.Empty;

            // Submission judge endpoints
            if (path.StartsWith("/api/submissions/") &&
                (path.EndsWith("/start") ||
                 path.EndsWith("/complete") ||
                 path.EndsWith("/fail") ||
                 path.EndsWith("/cancel") ||
                 path.EndsWith("/enqueue")))
                return true;

            // Problem judge endpoints
            if (path.EndsWith("/judge"))
                return true;

            return false;
        }
    }
}