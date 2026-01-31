using System.Net.Http.Json;
using VAlgo.JudgeWorker.Execution.Models;
using VAlgo.JudgeWorker.Models;

namespace VAlgo.JudgeWorker.Clients
{
    public sealed class SubmissionsApiClient
    {
        private readonly HttpClient _http;

        public SubmissionsApiClient(HttpClient http)
        {
            _http = http;
        }

        public Task<SubmissionDto?> Get(Guid id)
        {
            return _http.GetFromJsonAsync<SubmissionDto>($"api/submissions{id}");
        }

        public Task Start(Guid id)
        {
            return _http.PostAsync($"api/submissions/{id}/start", null);
        }

        public Task Complete(Guid id, JudgeOutcome outcome)
        {
            return _http.PostAsJsonAsync($"api/submissions/{id}/complete", new
            {
                Verdict = outcome.Verdict,
                PassedTestCases = outcome.Passed,
                TotalTestCases = outcome.Total,
                TimeMs = outcome.TimeMs,
                MemoryKb = outcome.MemoryKb
            });
        }

        public Task Fail(Guid id, string reason)
        {
            return _http.PatchAsJsonAsync($"api/submissions/{id}/fail", new
            {
                Reason = reason
            });
        }
    }
}