using System.Net.Http.Json;
using VAlgo.JudgeWorker.Clients.Models;
using VAlgo.JudgeWorker.Execution.Models;

namespace VAlgo.JudgeWorker.Clients
{
    public sealed class SubmissionsClient
    {
        private readonly HttpClient _http;

        public SubmissionsClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<SubmissionDto> GetSubmissionAsync(Guid submissionId, CancellationToken cancellationToken)
        {
            var response = await _http.GetAsync($"/api/submissions/{submissionId}", cancellationToken);
            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<SubmissionDto>(cancellationToken))!;
        }

        public async Task StartAsync(Guid submissionId, CancellationToken cancellationToken)
        {
            var response = await _http.PostAsync($"/api/submissions/{submissionId}/start", content: null, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task CompleteAsync(Guid submissionId, Verdict verdict, int passed, int total, int timeMs, int memoryKb, CancellationToken cancellationToken)
        {
            var payload = new
            {
                Verdict = verdict,
                PassedTestCases = passed,
                TotalTestCases = total,
                TimeMs = timeMs,
                MemoryKb = memoryKb
            };

            var response = await _http.PostAsJsonAsync($"/api/submissions/{submissionId}/complete", payload, cancellationToken);

            response.EnsureSuccessStatusCode();
        }

        public async Task FailAsync(Guid submissionId, string reason, CancellationToken cancellationToken)
        {
            var payload = new { Reason = reason };

            var response = await _http.PostAsJsonAsync($"/api/submissions/{submissionId}/fail", cancellationToken);

            response.EnsureSuccessStatusCode();
        }

        public async Task CancelAsync(Guid submissionId, CancellationToken cancellationToken)
        {
            var response = await _http.PostAsync($"/api/submissions/{submissionId}/cancel", content: null, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
    }
}