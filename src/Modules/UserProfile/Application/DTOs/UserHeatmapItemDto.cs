namespace VAlgo.Modules.UserProfile.Application.DTOs
{
    public sealed class UserHeatmapDto
    {
        public IReadOnlyList<UserHeatmapItemDto> Items { get; init; } = null!;
        public IReadOnlyList<int> AvailableYears { get; init; } = null!;
        public int SelectedYear { get; init; }
        public int TotalSubmissions { get; init; } //  Tổng Submission trong năm
        public int TotalActiveDays { get; init; } // Số ngày khác nhau submission
    }

    public sealed class UserHeatmapItemDto
    {
        public DateOnly Date { get; init; }
        public int Count { get; init; }
    }
}