# System Architecture

VAlgo follows **Clean Architecture** combined with **Domain Driven Design (DDD)** and **Modular Monolith architecture**.

The goal is to create a maintainable and scalable backend system.

---

# Clean Architecture

Each module is divided into layers:

```
API
Application
Domain
Infrastructure
```

Responsibilities:

| Layer          | Responsibility                 |
| -------------- | ------------------------------ |
| API            | HTTP endpoints                 |
| Application    | Use cases (commands / queries) |
| Domain         | Business rules                 |
| Infrastructure | Database / external systems    |

---

# Modular Monolith

Instead of building microservices, VAlgo uses **modular monolith architecture**.

Advantages:

* Easier development
* Clear module boundaries
* Independent domain models
* Can evolve into microservices later

Modules communicate using:

```
Domain Events
Application Services
```

---

# Domain Driven Design

The system is divided into **bounded contexts**.

Each bounded context has its own:

* domain model
* repository
* database mappings

This prevents tight coupling between modules.

---

# Event Driven Integration

Modules interact using **Domain Events**.

Example:

```
SubmissionCompletedDomainEvent
        │
        ▼
Contest Module
        │
        ▼
Update Leaderboard
```

This approach ensures:

* Loose coupling
* Clear responsibilities
* Scalable architecture
