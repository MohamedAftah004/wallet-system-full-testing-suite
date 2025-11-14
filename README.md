# 🧪 Wallet System — Comprehensive Testing Project (.NET 8)

A backend case-study project focused entirely on **writing professional, maintainable, and fully isolated automated tests** using **Clean Architecture** and modern testing practices.

The goal of this repository is **testing quality**, not building a full wallet product.

This project includes complete test coverage for:

- **Domain Layer**
- **Application Layer**
- **Infrastructure Layer**

The wallet system serves as a practical example to demonstrate real-world test design, mocking patterns, and clean testing principles.

---

## 🎯 Project Purpose

> Build a real backend system that is fully covered with automated tests following Clean Architecture principles.

This repository helps developers learn:

- How to write clean and reliable unit tests  
- Testing CQRS (Commands & Queries) with MediatR  
- Testing EF Core repositories  
- Mocking external services and dependencies  
- Isolating architectural layers  
- Structuring a test solution used in real production systems  

---

## 📁 Testing Coverage Overview

### 1️⃣ Domain Layer Tests
- Entity behaviors  
- Value object validation  
- Business rules  
- Purely isolated logic  
- Fast, dependency-free tests  

### 2️⃣ Application Layer Tests (CQRS)
Covers:

- Command handlers  
- Query handlers  
- FluentValidation validators  
- MediatR pipeline behavior  
- Business use-case execution  
- Error & exception handling  

Using **Moq** for mocking:

- Repositories  
- JWT token service  
- Password hasher  
- Unit of Work  
- Any infrastructure dependency  

### 3️⃣ Infrastructure Layer Tests
Covers:

- EF Core repositories  
- DbContext behavior  
- Data transactions  
- SaveChanges logic  

Using **EF Core InMemory** to test database behavior **without a real database**.

---

## 🧱 Technology Stack (Testing Focused)

| Category | Tools |
|---------|--------|
| Unit Testing | xUnit |
| Mocking | Moq |
| Assertions | FluentAssertions |
| Validation | FluentValidation |
| Database Testing | EF Core InMemory |
| Architecture | Clean Architecture |
| Patterns | CQRS, Repository, MediatR |

---

## 🧩 Project Structure

```
wallet-system/
│
├── backend/                     # Clean Architecture backend
│   ├── Wallet.Api
│   ├── Wallet.Application
│   ├── Wallet.Domain
│   └── Wallet.Infrastructure
│
├── tests/                       # Core focus — all test layers
│   ├── Wallet.Tests.Domain
│   ├── Wallet.Tests.Application
│   └── Wallet.Tests.Infrastructure
│
```

---

## 🚀 Running the Tests

```bash
cd tests
dotnet test
```

- Tests run fully standalone  
- No database required  
- No API required  

---

## 🧠 Why This Project Matters

This repository demonstrates:

- Real-world backend testing practices  
- Cleanly structured and isolated test layers  
- How to test core backend concepts:
  - CQRS  
  - MediatR behavior  
  - Domain logic & value objects  
  - Repository behavior  
  - EF Core operations  
  - Authentication flows  
- Helps you gain confidence for backend engineering roles  

---

## 📦 Optional: Running the Backend API

If you want to test the API manually:

```bash
cd backend/Wallet.Api
dotnet run
```

Swagger UI:  
https://localhost:7124/swagger

(Running the backend is optional — the project is designed to be test-first.)

---

## 👨‍💻 Author

**Mohamed Aftah**  
Backend Developer — (.NET | Testing | Clean Architecture)

📧 Email: mohamedaftah04@gmail.com  
🔗 GitHub: https://github.com/MohamedAftah004  
🔗 LinkedIn: https://www.linkedin.com/in/mabd-elfattah/
