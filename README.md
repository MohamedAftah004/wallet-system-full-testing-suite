# 🧪 Wallet System — Comprehensive Testing Project (.NET 8)

This repository focuses on building a **fully tested backend system** using **Clean Architecture** and **Unit Testing Best Practices**.

The main goal of the project is **testing**, not building a wallet application.

✔ You will find complete and professional test coverage for:

- **Domain Layer**
- **Application Layer**
- **Infrastructure Layer**

The wallet system itself exists only as a **practical case study** to demonstrate high-quality test architecture, mocking strategies, test isolation, and real-world scenarios.

---

## 🎯 Project Goal

The core purpose is:

> **To build a real backend project that is fully covered with automated tests following Clean Architecture principles.**

This repository is perfect for anyone wanting to learn:

- How to write clean and maintainable tests  
- How to test CQRS + MediatR  
- How to test EF Core repositories  
- How to mock external services  
- How to isolate layers properly  
- How to structure a test solution for real production apps  

---

# 🧪 Testing Coverage (Main Focus)

## ✔ 1️⃣ Domain Layer Tests
- Entities behavior  
- Value objects validation  
- Business rules  
- Pure logic without dependencies  
- 100% isolated and fast tests  

💡 *Domain tests verify that your core business rules never break.*

---

## ✔ 2️⃣ Application Layer Tests (CQRS)
Includes tests for:

- Command Handlers  
- Query Handlers  
- Validators (FluentValidation)  
- MediatR behavior  
- Business use-cases  
- Exception handling  

💡 Using **Moq** to mock:
- Repositories  
- JWT Token Service  
- Password Hasher  
- Unit of Work  
- External dependencies  

💡 These tests ensure the **business logic is correct** regardless of infrastructure/database.

---

## ✔ 3️⃣ Infrastructure Layer Tests
Covers:

- EF Core Repositories  
- DbContext interactions  
- Transactions logic  
- Data access patterns  

Using **EF Core InMemory Provider** to test database logic **without an actual database**.

💡 Ensures all data operations behave as expected.

---

# 🧱 Technology Stack (Testing-Oriented)

| Layer | Tools / Libraries |
|-------|------------------|
| **Unit Testing Framework** | xUnit |
| **Mocking Framework** | Moq |
| **Assertions** | FluentAssertions |
| **Validation** | FluentValidation |
| **In-Memory Database** | EF Core InMemory |
| **Architecture** | Clean Architecture |
| **Patterns** | CQRS, Repository, MediatR |

---

# 🧩 Project Structure

