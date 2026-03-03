# Bounded Context

## I. Bounded Context đề xuất cho VAlgo
1. Identity & Access Context

Chịu trách nhiệm:
    - User
    - Authentication
    - Authorization
    - Roles (admin / user)

KHÔNG chứa:
    - Profile xã hội
    - Ranking
    - Submission

2. Problem Management Context

Chịu trách nhiệm:
    - Problem
    - Testcases
    - Constraints
    - Tags
    - Difficulty

KHÔNG chứa:
    - Submission
    - Score
    - Leaderboard

3. Problem Classification

Chịu trách nhiệm:
    - Tag
    - Category
    - ProblemTag
    - ProblemCategory
    - Topics
    - Campine

4. Submission Context (CORE DOMAIN)

Chịu trách nhiệm:
   - Submission
   - SourceCode
   - Language
   - ExecutionResult
   - Status (Queued, Running, Accepted, Wrong Answer...)

KHÔNG chứa:
    - Ranking
    - Contest score
    - User profile

5. Judge / Execution Context

Chịu trách nhiệm:
    - Compile
    - Execute
    - Sandbox
    - Resource limits

Context này:
    - Stateless
    - Tách worker
    - Scale độc lập

6. Contest Context

Chịu trách nhiệm:
    - Contest
    - ContestProblem
    - Time window
    - Rules

7. Leaderboard / Ranking Context

Chịu trách nhiệm:
    - Ranking
    - Score
    - Penalty
    - Standings

8. Discussion / Community Context

Chịu trách nhiệm:
    - Discussion
    - Comment
    - Editorial

Sơ đồ tổng thể

User
 │
 ▼
Problem ──► Submission ──► JudgeWorker
                     │
                     ▼
               ExecutionResult
                     │
          ┌──────────┴─────────┐
          ▼                    ▼
     Contest              Leaderboard

## II. Mapping Bounded Context → Module

| Bounded Context    | Module      | Priority   |
| ------------------ | ----------- | -------- |
| Identity           | Identity    | ⭐        |
| Problem Management | Problems    | ⭐        |
| Submission         | Submissions | ⭐⭐⭐      |
| Judge / Execution  | JudgeWorker | ⭐⭐⭐      |
| Contest            | Contests    | ⭐⭐       |
| Leaderboard        | Leaderboard | ⭐        |
| Discussion         | Discussions | ⭐        |

## III. Tên gọi Ubiquitous Language

| Term              | Ý nghĩa                  |
| ----------------- | ------------------------ |
| Problem           | Một bài toán             |
| Submission        | Một lần nộp              |
| Execution         | Một lần chạy             |
| Verdict           | Kết quả chấm             |
| ContestSubmission | Submission trong contest |
| Judge             | Hệ thống chấm            |


