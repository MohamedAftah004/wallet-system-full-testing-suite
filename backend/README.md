# Wallet Backend — Clean Architecture • CQRS • .NET 8

(Backend designed for full Test Coverage)

This backend is part of the **Wallet System Testing Project**, built using **.NET 8**, **Clean Architecture**, and **CQRS with MediatR**.

The main purpose of this backend is to provide a clean, modular, and **test-friendly architecture** that supports:

- ✔ Unit Testing (Domain / Application / Infrastructure)
- ✔ Mocking external dependencies
- ✔ Isolated and maintainable business logic
- ✔ Real-world scenarios for wallet, users, and transactions

---

## 🧭 Architecture Overview

The backend follows **Clean Architecture**, separating all concerns into four layers:

```
[ Presentation ]   → Wallet.Api
[ Application ]    → CQRS Handlers, Validation, Interfaces
[ Domain ]         → Entities, Value Objects, Business Rules
[ Infrastructure ] → EF Core, Repositories, JWT, Persistence
```

### **Layer Responsibilities**

| Layer              | Responsibility                               |
| ------------------ | -------------------------------------------- |
| **API**            | REST Controllers + Request handling          |
| **Application**    | Commands/Queries, validation, business rules |
| **Domain**         | Core models, entities, value objects         |
| **Infrastructure** | EF Core, JWT, database access                |

This structure ensures **high testability**, **minimal coupling**, and **clear separation of business logic**.

---

## ⚙️ Tech Stack

| Category           | Technology                        |
| ------------------ | --------------------------------- |
| **Framework**      | ASP.NET Core 8 Web API            |
| **Architecture**   | Clean Architecture + CQRS         |
| **Mediator**       | MediatR                           |
| **Database**       | EF Core (PostgreSQL / SQL Server) |
| **Validation**     | FluentValidation                  |
| **Authentication** | JWT Bearer                        |
| **Patterns**       | Repository Pattern                |
| **DI**             | Built‑in .NET DI                  |

---

## 📂 Project Structure

```
src/
│
├── Wallet.Api/                 # Presentation layer
│
├── Wallet.Application/         # CQRS, validation, interfaces
│
├── Wallet.Domain/              # Entities, enums, rules
│
└── Wallet.Infrastructure/      # EF Core, JWT, repository implementations
tests/
│
├── Wallet.Application.Tests/        
│
├── Wallet.Domain.Tests/           
│
└── Wallet.Infrastructure.Tests/     
```

This clean setup allows each layer to be **tested independently**.

---

## 🔐 Core Backend Features

- User Authentication (JWT)
- Wallet creation and balance retrieval
- Top-up, payment, and refund transactions
- Transaction history & filtering
- Admin-level data queries

All features are designed to be **fully testable** with clear CQRS boundaries.

---

## 🧪 Testing Overview

This backend was structured specifically to support **full test coverage** across all layers:

### ✔ **Domain Tests**

- Pure business logic
- Entities, Value Objects, rules, exceptions
- No external dependencies

### ✔ **Application Tests**

- Commands, Queries, Handlers
- MediatR behavior
- Validation tests
- Mocking:
  - Repositories
  - JWT Service
  - Password Hasher
  - Unit of Work

### ✔ **Infrastructure Tests**

- EF Core InMemory tests
- Repository behaviors
- Database rule enforcement
- Query performance patterns

Testing goals:

- High code coverage
- No business logic in controllers
- Fully isolated tests

---

## 🚀 Running the Backend (Optional)

> **Note:** Running the API is optional — the backend is designed mainly for testing practice.

### 1️⃣ Restore Dependencies

```
dotnet restore
```

### 2️⃣ Apply Migrations (if needed)

```
dotnet ef database update
```

### 3️⃣ Run the API

```
dotnet run --project Wallet.Api
```

Swagger UI:
👉 https://localhost:7124/swagger

---

## 👨‍💻 Author

**Mohamed Aftah**  
Backend Developer — (.NET | Testing | Clean Architecture)

📧 Email: **mohamedaftah04@gmail.com**  
🔗 GitHub: **https://github.com/MohamedAftah004**  
🔗 LinkedIn: **https://www.linkedin.com/in/mabd-elfattah/**

---
