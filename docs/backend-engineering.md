# Backend Engineering

This document explains the **backend engineering principles and patterns** used in the VAlgo system.

The goal of the architecture is to build a **maintainable, scalable, and production-ready backend system**.

---

# Clean Architecture

VAlgo follows **Clean Architecture** to separate responsibilities and maintain clear boundaries between layers.

Each module is structured as:

```
API
Application
Domain
Infrastructure
```

### Responsibilities

| Layer          | Responsibility                        |
| -------------- | ------------------------------------- |
| API            | HTTP endpoints and request handling   |
| Application    | Use cases (commands and queries)      |
| Domain         | Core business logic and rules         |
| Infrastructure | Database access and external services |

The Domain layer is independent from infrastructure concerns.

---

# Domain Driven Design (DDD)

The system is designed using **Domain Driven Design** concepts.

The application is divided into **bounded contexts**, each responsible for a specific business domain.

Examples of bounded contexts in VAlgo:

* Identity
* Problems
* Submissions
* Contests
* Leaderboard
* Discussions

Each context contains its own:

* domain models
* repositories
* database mappings

This ensures **low coupling between modules**.

---

# Modular Monolith Architecture

Instead of microservices, VAlgo uses a **Modular Monolith architecture**.

Benefits:

* simpler deployment
* easier development
* clear domain boundaries
* easier refactoring

Project structure:

```
src
 ├── VAlgo.API
 ├── Modules
 │    ├── Problems
 │    ├── Submissions
 │    ├── Contests
 │    ├── Leaderboard
 │    ├── Discussions
 │
 └── SharedKernel
```

Modules interact through **events or application services**, not direct database access.

---

# CQRS (Command Query Responsibility Segregation)

VAlgo applies **CQRS** using **MediatR**.

Commands modify the system state, while queries read data.

### Commands

Examples:

```
CreateSubmission
CompleteSubmission
CreateDiscussion
AddComment
LockDiscussion
```

Commands:

* change system state
* contain business logic
* return minimal data

### Queries

Examples:

```
GetSubmissionDetail
GetContestLeaderboard
GetProblemDiscussions
GetDiscussionComments
```

Queries:

* do not modify state
* optimized for reading
* return DTOs

CQRS helps maintain **clear separation between read and write operations**.

---

# Domain Events

Domain events are used to communicate between modules.

Example event:

```
SubmissionCompletedDomainEvent
```

Flow:

```
Submission Module
        │
        ▼
SubmissionCompletedDomainEvent
        │
        ▼
Contest Module
        │
        ▼
Update participant score
```

Benefits:

* loose coupling between modules
* clear event-driven flow
* easier extensibility

---

# Repository Pattern

Repositories abstract data access logic from the domain.

Example:

```
IContestRepository
ISubmissionRepository
IDiscussionRepository
```

Responsibilities:

* load aggregates
* persist aggregates
* hide database details

This keeps the **domain layer independent from EF Core**.

---

# Unit of Work

Unit of Work ensures that multiple operations are saved as a single transaction.

Example:

```
CreateSubmission
AddTestCaseResults
UpdateSubmissionStatus
```

All changes are committed together.

This guarantees **data consistency**.

---

# Aggregate Design

The system follows **DDD aggregate rules**.

Examples:

### Submission (Aggregate Root)

```
Submission
 └── TestCaseResults
```

### Discussion (Aggregate Root)

```
Discussion
 └── Comments
```

All modifications must go through the aggregate root.

This protects domain invariants.

---

# Event Driven Integration

Modules communicate through **domain events** instead of direct dependencies.

Example:

```
SubmissionCompletedDomainEvent
        │
        ▼
Contest.ProcessSubmission()
        │
        ▼
Leaderboard update
```

This approach improves:

* modularity
* extensibility
* maintainability

---

# Real-time Systems

Real-time leaderboard updates are implemented using **SignalR**.

Flow:

```
Submission Completed
        │
        ▼
Contest Score Updated
        │
        ▼
SignalR Broadcast
        │
        ▼
Frontend Receives Update
```

This allows users to see **live contest rankings**.

---

# Caching Strategy

Redis can be used to cache frequently accessed data.

Examples:

* contest leaderboard
* problem lists
* discussion lists

Caching reduces database load and improves response time.

---

# Security Considerations

Security measures include:

* sandboxed code execution
* resource limits (CPU / memory)
* isolated execution environment

User code is executed inside **Docker containers** to prevent system compromise.

---

# Scalability Considerations

The architecture supports future scaling.

Possible improvements:

* distributed judge workers
* message queues for submission processing
* microservice extraction for heavy modules
* horizontal API scaling

The current modular architecture allows the system to evolve gradually.

---

# Engineering Goals

The backend architecture aims to achieve:

* maintainable codebase
* clear domain boundaries
* scalable submission processing
* reliable contest ranking
* extensible system design
