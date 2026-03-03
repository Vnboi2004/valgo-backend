# Tổng quan luồng hoạt động của JudgeWorker

User
 ↓
API (Submissions)
 ↓
SubmissionCreatedDomainEvent
 ↓
EnqueueSubmission
 ↓
Message Queue (RabbitMQ / Kafka / Azure Service Bus)
 ↓
JudgeWorker
 ↓
Execute code in Sandbox
 ↓
Report result back to API

## Flow chi tiết
Bước 1 - User submit 
Submission.Create(...)
-> SubmissionCreateDomainEvent()

Bước 2: enqueue job
    -> Submission.Enqueue(now)
    ->  Publish JudgeJobMessage

Bước 3: judgeWorker nhận job
GET /api/problems/{id}/judge
GET /api/submissions/{id}

Bước 4: Start submission
POST /api/submissions/{id}/start


Bước 5: Execute từng testcase (sandbox)

for testCase in problem.TestCases:
    run code in sandbox
    collect:
      - verdict
      - time
      - memory
    POST AddTestCaseResult
    if verdict != Accepted:
        break

Bước 6: Complete / Fail submisson

POST /api/submissions/{id}/complete
{
  verdict,
  totalTestCases,
  passedTestCases,
  timeMs,
  memoryKb
}





