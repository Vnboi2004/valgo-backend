# 1. Use case nhóm 1 — Contest lifecycle (Admin)

Đây là các hành động quản lý contest.
Commands

| Use case                     | Mô tả                   |
| ---------------------------- | ----------------------- |
| CreateContest                | tạo contest             |
| UpdateContestMetadata        | sửa title / description |
| UpdateContestSchedule        | sửa start / end time    |
| UpdateContestVisibility      | public / private        |
| UpdateContestMaxParticipants | giới hạn người tham gia |

Lifecycle
| Use case       | Mô tả            |
| -------------- | ---------------- |
| PublishContest | publish contest  |
| StartContest   | bắt đầu contest  |
| FinishContest  | kết thúc contest |
| ArchiveContest | lưu trữ contest  |

# 2. Use case nhóm 2 - Contest problems (Admin)

Admin thêm/xóa bài vào contest.

| Use case                   | Mô tả        |
| -------------------------- | ------------ |
| AddProblemToContest        | thêm problem |
| RemoveProblemFromContest   | xóa problem  |
| ReorderContestProblems     | đổi thứ tự   |
| UpdateContestProblemPoints | chỉnh điểm   |


# 3. Use case nhóm 3 - Participants (User)

Người tham gia contest

| Use case     | Mô tả            |
| ------------ | ---------------- |
| JoinContest  | tham gia contest |
| LeaveContest | rời contest      |


# 4. Queries

Queries để hiển thị UI

Contest
| Query            | Mô tả             |
| ---------------- | ----------------- |
| GetContests      | danh sách contest |
| GetContestDetail | chi tiết contest  |

Contest Problems
| Query              | Mô tả         |
| ------------------ | ------------- |
| GetContestProblems | list problems |

Participants
| Query                  | Mô tả             |
| ---------------------- | ----------------- |
| GetContestParticipants | list participants |

Leaderboard
| Query                 | Mô tả         |
| --------------------- | ------------- |
| GetContestLeaderboard | bảng xếp hạng |

User contest
| Query         | Mô tả                 |
| ------------- | --------------------- |
| GetMyContests | contest user tham gia |

