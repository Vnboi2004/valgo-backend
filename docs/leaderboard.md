# Leaderboard System

The leaderboard displays contest rankings.

It updates automatically when submissions finish.

---

# Ranking Rule

Participants are ranked using:

```
1. Score (higher is better)
2. Penalty (lower is better)
```

Example:

| Rank | User  | Score | Penalty |
| ---- | ----- | ----- | ------- |
| 1    | UserA | 4     | 350     |
| 2    | UserB | 4     | 420     |
| 3    | UserC | 3     | 300     |

---

# Score Calculation

When a submission is accepted:

```
Score += 1
Penalty += submission time + wrong attempts penalty
```

---

# Real-time Updates

Leaderboard updates when:

```
SubmissionCompletedDomainEvent
```

Flow:

```
Submission Module
        │
        ▼
Domain Event
        │
        ▼
Contest Module updates participant
        │
        ▼
SignalR broadcast
        │
        ▼
Frontend receives update
```

---

# Performance Optimization

To support large contests:

* Leaderboard caching
* Efficient sorting
* Incremental updates
