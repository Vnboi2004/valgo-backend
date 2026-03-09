namespace VAlgo.API.Controllers.Contests.Requests
{
    public sealed record UpdateContestMaxParticipantsRequest(
        int? MaxParticipants
    );
}