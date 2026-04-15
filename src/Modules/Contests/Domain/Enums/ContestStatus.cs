namespace VAlgo.Modules.Contests.Domain.Enums
{
    public enum ContestStatus
    {
        Draft = 1, // Contest mới tạo, chưa được công bố
        Published = 2, // Contest đã được công bố, người dùng có thể đăng ký tham gia
        Running = 3, // Contest đang diễn ra, người dùng đã đăng ký có thể tham gia thi đấu
        Finished = 4, // Contest đã kết thúc, người dùng không thể tham gia thi đấu nữa
        Archived = 5  // Contest đã được lưu trữ, không hiển thị trên giao diện người dùng nhưng vẫn giữ lại dữ liệu cho mục đích thống kê hoặc lịch sử
    }
}