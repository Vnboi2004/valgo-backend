using System.Net.Http.Json;
using VAlgo.JudgeWorker.Clients.Models;

namespace VAlgo.JudgeWorker.Clients
{
    public sealed class ProblemsClient
    {
        private readonly HttpClient _http;

        public ProblemsClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<ProblemForJudgeDto> GetProblemForJudgeAsync(Guid problemId, CancellationToken cancellationToken)
        {
            var response = await _http.GetAsync($"/api/problems/{problemId}/judge", cancellationToken);
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadFromJsonAsync<ProblemForJudgeDto>(cancellationToken))!;
        }
    }
}