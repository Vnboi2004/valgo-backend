using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using VAlgo.JudgeWorker.Models;
using VAlgo.JudgeWorker.Services;

namespace VAlgo.JudgeWorker.Clients
{
    public sealed class VAlgoApiClient
    {
        private readonly HttpClient _http;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        public VAlgoApiClient(HttpClient http)
        {
            _http = http;
        }

        // Submissions
        // GET api/submissions/{submissionId}
        public async Task<SubmissionDto> GetSubmissionAsync(Guid submissionId, CancellationToken cancellationToken = default)
        {
            var response = await _http.GetAsync($"/api/submissions/{submissionId}", cancellationToken);

            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<SubmissionDto>(JsonOptions, cancellationToken))!;
        }

        // GET api/problems/{problemId}/templates/{language}/judge
        public async Task<CodeTemplateForJudgeDto> GetCodeTemplateForJudgeAsync(Guid problemId, string language, CancellationToken cancellationToken = default)
        {
            var response = await _http.GetAsync($"/api/problems/{problemId}/templates/{language}/judge", cancellationToken);

            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<CodeTemplateForJudgeDto>(
                JsonOptions, cancellationToken))!;
        }

        // POST api/submissions/{submissionId}/start
        public async Task StartSubmissionAsync(Guid submissionId, CancellationToken cancellationToken = default)
        {
            var response = await _http.PostAsync($"/api/submissions/{submissionId}/start", content: null, cancellationToken);

            response.EnsureSuccessStatusCode();
        }

        // POST api/submissions/{submissionId}/complete
        public async Task CompleteSubmissionAsync(Guid submissionId, CompleteSubmissionRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _http.PostAsJsonAsync($"/api/submissions/{submissionId}/complete", request, cancellationToken);

            response.EnsureSuccessStatusCode();
        }

        // POST api/submissions/{submissionId}/fail
        public async Task FailSubmissionAsync(Guid submissionId, FailSubmissionRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _http.PostAsJsonAsync($"/api/submissions/{submissionId}/fail", request, cancellationToken);

            response.EnsureSuccessStatusCode();
        }

        // Problems
        public async Task<ProblemForJudgeDto> GetProblemForJudgeAsync(Guid problemId, CancellationToken cancellationToken = default)
        {
            var response = await _http.GetAsync($"/api/problems/{problemId}/judge", cancellationToken);

            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<ProblemForJudgeDto>(JsonOptions, cancellationToken))!;
        }
    }
}