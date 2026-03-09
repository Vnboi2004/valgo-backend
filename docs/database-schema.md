# Database Schema

VAlgo uses **PostgreSQL** as the primary relational database.

The database is designed following **Domain Driven Design aggregates**.

Each module manages its own entities.

---

# Core Tables

The main entities in the system include:

```text
Users
Problems
Submissions
Contests
ContestParticipants
ContestProblems
Discussions
Comments
```

---

# Users

Stores user accounts.

| Column        | Type      |
| ------------- | --------- |
| id            | uuid      |
| username      | text      |
| email         | text      |
| password_hash | text      |
| created_at    | timestamp |

---

# Problems

Stores programming problems.

| Column          | Type      |
| --------------- | --------- |
| id              | uuid      |
| title           | text      |
| description     | text      |
| difficulty      | int       |
| time_limit_ms   | int       |
| memory_limit_mb | int       |
| created_at      | timestamp |

---

# TestCases

Each problem has multiple test cases.

| Column          | Type    |
| --------------- | ------- |
| id              | uuid    |
| problem_id      | uuid    |
| input           | text    |
| expected_output | text    |
| is_sample       | boolean |

---

# Submissions

Stores user code submissions.

| Column      | Type      |
| ----------- | --------- |
| id          | uuid      |
| user_id     | uuid      |
| problem_id  | uuid      |
| language    | int       |
| source_code | text      |
| status      | int       |
| verdict     | int       |
| created_at  | timestamp |
| finished_at | timestamp |

---

# TestCaseResults

Stores execution results for each test case.

| Column        | Type |
| ------------- | ---- |
| id            | uuid |
| submission_id | uuid |
| index         | int  |
| verdict       | int  |
| time_ms       | int  |
| memory_kb     | int  |

---

# Contests

Stores contest information.

| Column     | Type      |
| ---------- | --------- |
| id         | uuid      |
| title      | text      |
| start_time | timestamp |
| end_time   | timestamp |
| created_at | timestamp |

---

# ContestProblems

Mapping between contests and problems.

| Column      | Type |
| ----------- | ---- |
| id          | uuid |
| contest_id  | uuid |
| problem_id  | uuid |
| order_index | int  |

---

# ContestParticipants

Stores contest participants and scores.

| Column     | Type |
| ---------- | ---- |
| id         | uuid |
| contest_id | uuid |
| user_id    | uuid |
| score      | int  |
| penalty    | int  |

---

# Discussions

Discussion threads for problems.

| Column        | Type      |
| ------------- | --------- |
| id            | uuid      |
| problem_id    | uuid      |
| author_id     | uuid      |
| title         | text      |
| content       | text      |
| is_locked     | boolean   |
| comment_count | int       |
| created_at    | timestamp |
| updated_at    | timestamp |

---

# Comments

Comments inside discussions.

| Column            | Type      |
| ----------------- | --------- |
| id                | uuid      |
| discussion_id     | uuid      |
| author_id         | uuid      |
| parent_comment_id | uuid      |
| content           | text      |
| created_at        | timestamp |
| updated_at        | timestamp |

---

# Relationships

Key relationships in the system:

```text
User
 ├── Submissions
 ├── ContestParticipants
 └── Discussions

Problem
 ├── TestCases
 ├── Submissions
 └── Discussions

Contest
 ├── ContestProblems
 └── ContestParticipants

Discussion
 └── Comments
```

---

# Indexing Strategy

Important indexes include:

```text
submissions(problem_id)
submissions(user_id)
contest_participants(contest_id)
comments(discussion_id)
discussions(problem_id)
```

Indexes improve performance for:

* leaderboard queries
* submission history
* discussion pagination

---

# Design Principles

Database schema design focuses on:

* normalized data structure
* clear foreign key relationships
* efficient query performance
* support for contest ranking and submissions
