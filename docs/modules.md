# System Modules

VAlgo is divided into multiple **bounded contexts (modules)**.

Each module has a clear responsibility.

---

# Identity Module

Responsible for:

* User accounts
* Authentication
* Authorization
* Roles

---

# Problems Module

Responsible for:

* Problem statements
* Test cases
* Constraints
* Difficulty levels

---

# Problem Classification Module

Responsible for:

* Tags
* Categories
* Problem classification

---

# Submissions Module

Core domain of the system.

Responsible for:

* Code submissions
* Submission status
* Execution results
* Verdict management

Submission statuses:

```
Created
Queued
Running
Completed
Failed
Cancelled
```

---

# Contest Module

Responsible for:

* Contest creation
* Contest problems
* Participants
* Score calculation
* Penalty calculation

---

# Leaderboard Module

Responsible for:

* Ranking
* Standings
* Score ordering

Sorting rule:

```
Score DESC
Penalty ASC
```

---

# Discussions Module

Responsible for community interaction.

Features:

* Discussions per problem
* Comment system
* Nested replies
* Moderation

---

# Judge Worker

Separate service responsible for:

* Compiling code
* Running code
* Collecting results
* Returning verdicts
