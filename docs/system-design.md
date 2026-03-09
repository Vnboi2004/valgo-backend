# System Design

VAlgo is designed as a **scalable backend architecture for an Online Judge platform** similar to Codeforces or LeetCode.

The system focuses on handling:

* code submissions
* automated judging
* contest ranking
* real-time leaderboard updates

---

# High Level Architecture

The platform consists of several core components:

```text
Client (Frontend)
        │
        ▼
   ASP.NET API
        │
        ▼
Application Layer (CQRS + MediatR)
        │
        ▼
Domain Layer (Business Logic)
        │
        ▼
Infrastructure Layer (EF Core, Redis, PostgreSQL)
```

Additionally, a **Judge Worker service** processes code execution.

```text
User
 │
 ▼
Submission API
 │
 ▼
Submission Module
 │
 ▼
Queue / Pending Submission
 │
 ▼
Judge Worker
 │
 ▼
Execution Result
 │
 ▼
Submission Completed Event
 │
 ▼
Contest Module
 │
 ▼
Leaderboard Update
```

---

# Submission Processing Flow

When a user submits code:

```text
User submits solution
        │
        ▼
Submission entity created
        │
        ▼
Submission queued
        │
        ▼
Judge Worker picks submission
        │
        ▼
Compile source code
        │
        ▼
Execute test cases
        │
        ▼
Collect execution results
        │
        ▼
Update submission status
```

Possible verdicts:

* Accepted
* Wrong Answer
* Time Limit Exceeded
* Memory Limit Exceeded
* Runtime Error
* Compile Error

---

# Contest Scoring Flow

When a submission finishes:

```text
SubmissionCompletedDomainEvent
        │
        ▼
Contest Module receives event
        │
        ▼
Update participant score
        │
        ▼
Update penalty
        │
        ▼
Recalculate leaderboard
```

Penalty is calculated based on:

```text
submission_time + wrong_attempt_penalty
```

---

# Real-Time Leaderboard

Leaderboard updates are broadcast using **SignalR**.

Flow:

```text
Submission Completed
        │
        ▼
Contest Score Updated
        │
        ▼
SignalR Broadcast
        │
        ▼
Frontend receives update
```

This enables **live leaderboard updates during contests**.

---

# Caching Strategy

To improve performance:

* Redis cache for leaderboard queries
* Cache invalidation when new submissions complete

Example flow:

```text
Leaderboard Query
        │
        ▼
Check Redis Cache
        │
 ┌──────┴───────┐
 ▼              ▼
Cache Hit     Cache Miss
 │              │
Return data    Query database
               │
               ▼
            Update cache
```

---

# Scalability Considerations

The architecture is designed to support future scaling.

Possible improvements:

* Distributed Judge Workers
* Message queue for submissions
* Horizontal scaling of API servers
* Microservices migration

---

# Key Design Goals

The system design focuses on:

* clear module boundaries
* event-driven communication
* scalable submission processing
* real-time contest updates
* maintainable architecture
