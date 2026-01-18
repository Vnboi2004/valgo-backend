# A. Submission Creation Flow

### UC-01: CreateSubmission
Ai gọi: API
Mục đích: Người dùng nộp code

Xử lý:
    - Validate user
    - Validate problem tồn tại
    - Validate language hợp lệ
    - Tạo Submission
    - Save
    - Publish event

### UC-02: EnqueueSubmission
Ai gọi: API / background scheduler
Mục đích: Đưa submission vào hàng đợi judge

Xử lý:
    - Load submission
    - Ensure status = Created
    - submission.Enqueue()
    - Save
    - Push job → Queue

Tách riêng để:
    - Retry
    - Rate-limit
    - Delay judge

# B. Judge Execution Flow

### UC-03: StartSubmissionExecution
Ai gọi: JudgeWorker
Mục đích: Worker nhận job và bắt đầu chạy

Xử lý:
    - Load submission
    - Ensure status = Queued
    - submission.StartRunning()
    - Save

### UC-04: CompleteSubmission
Ai gọi: JudgeWorker
Mục đích: Kết thúc execution thành công

Xử lý:
    - Load submission
    - Ensure status = Running
    - submission.Complete(verdict, summary)
    - Save

### UC-05: FailSubmission
Ai gọi: JudgeWorker / System
Mục đích: Lỗi hệ thống, crash container, timeout infra

### UC-06: CancelSubmission
Ai gọi: User / System
Mục đích: Hủy submission khi:
    - contest kết thúc
    - user hủy
    - admin hủy

# C. Query Flow (Read Model)

### UC-07: GetSubmissionDetail
Ai gọi: API
Trả về:
    - Status
    - Verdict
    - JudgeSummary
    - Timestamps

### UC-08: GetSubmissionList
Ai gọi: API
Filter:
    - User
    - Problem
    - Status
    - Verdict
    - Date range

### UC-09: GetSubmissionStatus (Lightweight)
Ai gọi: Frontend polling
Trả về: Status + Verdict

# D. Advanced / Future-proof

### UC-10: RejudgeSubmission
Ai gọi: Admin / System
Khi:
    - Update testcases
    - Fix judge bug

### UC-11: DuplicateSubmissionDetection
Ai gọi: System
Dựa trên: SourceCodeHash

### UC-12: SubmissionRateLimit
Ai gọi: System
Chống spam
