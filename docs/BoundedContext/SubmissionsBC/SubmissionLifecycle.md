# Mục tiêu của Submission Lifecycle

- Rõ ràng trạng thái
- Không ambiguity
- Không bị “kẹt submission”
- Retry được
- Eventual consistency OK
- JudgeWorker scale ngang không vỡ

1. Submission
Submission = một yêu cầu chấm code bất biến về input (code + language + problem)

2. Danh sách trạng thái (Status)
    - Primary States:
        + Created
        + Queued
        + Running
        + Completed
        + Failed
        + Cancelled

    - Verdict:
        + Accepted
        + WrongAnswer
        + TimeLimitExceeded
        + MemoryLimitExceeded
        + RuntimeError
        + CompileError
        + SystemError

3. State Machine tổng thể

┌─────────┐
│ Created │
└────┬────┘
     │ enqueue
     ▼
┌─────────┐
│ Queued  │◄──────────────┐
└────┬────┘               │ retry
     │ picked by worker   │
     ▼                    │
┌─────────┐               │
│ Running │               │
└────┬────┘               │
     │ execution finished │
     ▼                    │
┌─────────────┐           │
│ Completed   │           │
│ + Verdict   │           │
└─────────────┘           │
     ▲                    │
     │ error              │
┌────┴────┐               │
│ Failed  │───────────────┘
└─────────┘

Cancelled (terminal, admin/user action)

4. Ý nghĩa từng trạng thái (Status)
- Created:
    + Submission vừa được tạo
    + Chưa publish message
    + Transaction boundary

- Queued:
    + Đã publish event
    + Đang chờ JudgeWorker

- Running:
    + Worker đã nhận submission
    + Đang compile / execute

- Completed:
    + Execution kết thúc
    + Có verdict cuối cùng

- Failed:
    + Lỗi hệ thống
    + Docker crash
    + Timeout infrastructure
    + Worker chết

- Cancelled:
    + Admin huỷ
    + Contest end
    + User huỷ (optional)


5. State Transition Rules

| From    | To        | Điều kiện       |
| ------- | --------- | --------------- |
| Created | Queued    | Publish success |
| Queued  | Running   | Worker nhận     |
| Running | Completed | Chấm xong       |
| Running | Failed    | Infra error     |
| Failed  | Queued    | Retry           |
| Any     | Cancelled | Admin action    |


6. Verdict Rules

| Verdict     | Khi nào       |
| ----------- | ------------- |
| Accepted    | Output đúng   |
| WrongAnswer | Output sai    |
| TLE         | Vượt time     |
| MLE         | Vượt memory   |
| RE          | Runtime crash |
| CE          | Compile fail  |
| SystemError | Lỗi judge     |







