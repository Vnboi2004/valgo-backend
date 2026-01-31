using System.Net.Http.Json;
using VAlgo.JudgeWorker.Models;

namespace VAlgo.JudgeWorker.Clients
{
    public sealed class ProblemsApiClient
    {
        private readonly HttpClient _http;

        public ProblemsApiClient(HttpClient http)
        {
            _http = http;
        }

        public Task<ProblemJudgeDto?> GetForJudge(Guid problemId)
        {
            return _http.GetFromJsonAsync<ProblemJudgeDto>($"api/problems/{problemId}/judge");
        }
    }
}