Step 1
Contest module (xong)

Step 2
Submission → Contest integration

Step 3
Submission module hoàn chỉnh

Step 4
JudgeWorker (Docker sandbox)

Step 5
Real-time leaderboard

Step 6
Caching leaderboard


Khi Submission
| Verdict          | Hành vi       |
| ---------------- | ------------- |
| Accepted lần đầu | +Points       |
| Accepted lần 2+  | Ignore        |
| WrongAnswer      | +20 penalty   |
| Accepted         | +time penalty |
