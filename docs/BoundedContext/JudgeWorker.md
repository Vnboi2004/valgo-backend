# MILESTONE 1 – CORE FLOW

Mục tiêu: 1 submission → chấm đúng → trả kết quả → không crash

# MILESTONE 2 – TEST END-TO-END

Mục tiêu: Judge chạy đúng flow

Việc cần làm:
    - 1 submission C++ → Accepted
    - 1 submission WA
    - 1 submission TLE
    - 1 submission Compile Error
    - Python + TimeMultiplier
    - Multiple testcases → stop sớm khi fail

# MILESTONE 3 – RELIABILITY (SỐNG ĐƯỢC)

Mục tiêu: Worker chết không làm hệ thống chết

Việc cần làm:
    - Retry policy:
        + Retry infra lỗi
        + Không retry user lỗi

    - Worker restart:
        + submission "Running" -> reset về "Queued"
    - Docker crash -> Fail đúng reason
    - Network API fail -> Retry + backoff

# MILESTONE 4 – TIMEOUT & RESOURCE CONTROL

Mục tiêu: Không submission nào chạy vô hạn

Việc cần làm:
    - Global submission timeout
    - Per-testcase timeout (đã có)
    - Memory cap enforce rõ ràng
    - Kill process khi timeout

# MILESTONE 5 – OPERABILITY (VẬN HÀNH)
Mục tiêu: Debug được khi có sự cố

Việc cần làm:
    - Structured logging:
        +  WorkerId
        + SubmissionId
        + Phase
    - Graceful shutdown:
        + Finish job đang chạy
    - Docker availability check lúc start
    - Health / readiness check

# MILESTONE 6 – SCALE & POLISH (FINAL)

Mục tiêu: Chạy worker không lỗi

Việc cần làm: 
    - Multiple workers cùng queue
    - Idempotent submission handling
    - Không double judge
    - Documentation:
        + Flow diagram
        + Failure scenarios
        + Operational notes