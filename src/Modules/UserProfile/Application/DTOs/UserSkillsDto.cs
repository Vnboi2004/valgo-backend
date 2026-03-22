namespace VAlgo.Modules.UserProfile.Application.DTOs
{
    public sealed class UserSkillsDto
    {
        public IReadOnlyList<UserSkillItemDto> Advanced { get; init; } = [];
        public IReadOnlyList<UserSkillItemDto> Intermediate { get; init; } = [];
        public IReadOnlyList<UserSkillItemDto> Fundamental { get; init; } = [];
    }

    public sealed class UserSkillItemDto
    {
        public Guid ClassificationId { get; init; }
        public string Name { get; init; } = null!;
        public int ProblemsSolved { get; init; }
    }
}