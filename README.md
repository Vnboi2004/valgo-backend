# VAlgo – Online Judge Platform

VAlgo is a **modern Online Judge backend platform** designed with **Clean Architecture, Domain-Driven Design (DDD), and Modular Monolith architecture**.

The system simulates a real competitive programming platform similar to **Codeforces**, **LeetCode**, or **HackerRank**.

This project focuses on **backend system design and architecture**, including submission processing, automated judging, contest systems, real-time leaderboards, and community discussions.

---

# System Overview

The platform supports the full lifecycle of competitive programming.

```
User
 │
 ▼
Problem ──► Submission ──► Judge Worker
                     │
                     ▼
               Execution Result
                     │
          ┌──────────┴──────────┐
          ▼                     ▼
       Contest             Leaderboard
                                │
                                ▼
                        Real-time Updates
```

---

# Architecture

The system follows **Clean Architecture + Domain Driven Design + Modular Monolith**.

Project structure:

```
src
 ├── VAlgo.API
 ├── Modules
 │    ├── Identity
 │    ├── Problems
 │    ├── ProblemClassification
 │    ├── Submissions
 │    ├── Contests
 │    ├── Leaderboard
 │    ├── Discussions
 │
 ├── JudgeWorker
 │
 └── SharedKernel
```

Each module contains:

```
Domain
Application
Infrastructure
```

---

# Key Features

### Problem System

* Create and manage programming problems
* Testcase management
* Problem difficulty
* Tags and categories

### Submission System

* Code submission
* Multiple programming languages
* Compile and execute code
* Detailed verdict results

Supported verdicts:

```
Accepted
Wrong Answer
Time Limit Exceeded
Memory Limit Exceeded
Runtime Error
Compile Error
System Error
```

---

# Contest System

Contest module provides:

* Contest creation
* Contest problems
* Contest participants
* Score calculation
* Penalty calculation
* Leaderboard ranking

Ranking rule:

```
Sort by:
1. Score (descending)
2. Penalty (ascending)
```

---

# Real-time Leaderboard

Leaderboard updates automatically when submissions finish.

Architecture:

```
Submission Completed
        │
        ▼
Domain Event
        │
        ▼
Contest Score Update
        │
        ▼
SignalR Broadcast
        │
        ▼
Frontend Leaderboard
```

---

# Discussion System

Each problem has a discussion forum.

Features:

* Create discussions
* Comment system
* Nested replies
* Discussion moderation
* Pagination

---

# Technology Stack

Backend:

* .NET 9
* ASP.NET Core Web API
* Entity Framework Core
* MediatR (CQRS)

Infrastructure:

* PostgreSQL
* Redis (for caching)
* Docker
* SignalR

Architecture:

* Clean Architecture
* Domain Driven Design
* Modular Monolith

---

# Engineering Concepts

This project demonstrates:

* Domain Driven Design
* CQRS with MediatR
* Repository Pattern
* Unit of Work
* Domain Events
* Event-driven module integration
* Real-time systems using SignalR
* Docker sandbox execution

---

# Documentation

More detailed documentation:

* [Architecture](docs/architecture.md)
* [Modules](docs/modules.md)
* [Judge System](docs/judge-system.md)
* [Leaderboard System](docs/leaderboard.md)

---

# Author

**Truong Phuoc Hung**

Backend Developer (.NET)

Focus areas:

* Backend Architecture
* Distributed Systems
* Competitive Programming Platforms
