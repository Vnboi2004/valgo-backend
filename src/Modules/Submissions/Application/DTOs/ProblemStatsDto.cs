namespace VAlgo.Modules.Submissions.Application.DTOs;

public sealed class ProblemStatsDto
{
    public int TotalSubmissions { get; init; }
    public int AcceptedSubmissions { get; init; }
    public double AcceptanceRate { get; init; }
}