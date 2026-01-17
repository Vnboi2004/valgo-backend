# VAlgo Backend

VAlgo Backend is the core backend service for **VAlgo – a Vietnamese algorithm and online judge platform** inspired by LeetCode.  
It handles problem management, code submissions, judging orchestration, and core business logic.

The system is designed with **Domain-Driven Design (DDD)** principles, using a **Modular Monolith** architecture combined with **distributed judge workers** for secure and scalable code execution.

---

## Features

- Algorithm problem & test case management
- Code submission lifecycle (queue → running → result)
- Distributed judge worker orchestration
- Secure code execution using Docker
- Clear domain boundaries with Modular Monolith architecture
- Extensible design for future scaling

---

## Architecture Overview

The backend follows a **Modular Monolith** architecture:

