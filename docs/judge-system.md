# Judge System

The judge system is responsible for compiling and executing user submissions.

It runs as a **separate worker service**.

---

# Submission Pipeline

```
User submits code
        │
        ▼
Submission Created
        │
        ▼
Submission Queued
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
Collect results
        │
        ▼
Return verdict
```

---

# Execution Sandbox

To ensure security, code execution runs inside a **sandbox environment**.

Sandbox limitations:

* CPU limit
* Memory limit
* Execution time limit
* File system isolation

Docker containers are used for sandbox execution.

---

# Test Case Execution

Each problem contains multiple test cases.

Execution steps:

```
Compile program
For each test case:
    Run program
    Compare output
    Record result
```

---

# Judge Summary

After execution, a summary is returned:

* Passed test cases
* Total test cases
* Execution time
* Memory usage
* Final verdict
