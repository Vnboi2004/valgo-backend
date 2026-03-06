# Contest (Aggregate Root)

| Field           | Type              |
| --------------- | ----------------- |
| Title           | string            |
| Description     | string            |
| StartTime       | DateTimeOffset    |
| EndTime         | DateTimeOffset    |
| Status          | ContestStatus     |
| Visibility      | ContestVisibility |
| MaxParticipants | int?              |
| CreatedBy       | Guid              |
| CreatedAt       | DateTimeOffset    |


# ContestProblem

| Field     | Type      |
| --------- | --------- |
| ContestId | ContestId |
| ProblemId | Guid      |
| Code      | string    |
| Order     | int       |
| Points    | int       |


# ContestParticipant

| Field            | Type            |
| ---------------- | --------------- |
| ContestId        | ContestId       |
| UserId           | Guid            |
| JoinedAt         | DateTimeOffset  |
| Score            | int             |
| Penalty          | int             |
| Rank             | int             |
| LastSubmissionAt | DateTimeOffset? |

# ContestSubmission

| Field        | Type |
| ------------ | ---- |
| Id           | Guid |
| ContestId    | Guid |
| SubmissionId | Guid |
| UserId       | Guid |
| ProblemId    | Guid |
| Verdict      | enum |
| Time         | int  |
| Penalty      | int  |

# LifeCycle

Draft
  │
  ▼
Published
  │
  ▼
Running
  │
  ▼
Finished
  │
  ▼
Archived

