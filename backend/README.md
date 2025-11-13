# 🧠 Wallet Backend — ASP.NET Core 8 • Clean Architecture • CQRS • MediatR

This is the **backend service** for the **Wallet System** — a digital wallet platform built with **ASP.NET Core 8**, following **Clean Architecture** and **CQRS** principles.  
It provides a modular, testable, and scalable API for handling users, wallets, and transactions securely using **JWT Authentication** and **Entity Framework Core**.

---

## 🧭 Overview

The **Wallet Backend** handles all business logic and data persistence.  
It’s organized into four cleanly separated layers:

```
[ Presentation ]   → Wallet.Api
[ Application ]    → Wallet.Application
[ Domain ]         → Wallet.Domain
[ Infrastructure ] → Wallet.Infrastructure
```

Each layer has a clear responsibility:

| Layer | Responsibility |
| ------ | --------------- |
| **API** | Exposes REST endpoints |
| **Application** | CQRS Handlers (Commands / Queries) |
| **Domain** | Business entities, enums, and core logic |
| **Infrastructure** | Data persistence, JWT, and external services |

---

## ⚙️ Tech Stack

| Category | Technology |
| --------- | ----------- |
| **Framework** | ASP.NET Core 8 Web API |
| **Database** | Entity Framework Core + SQL Server / PostgreSQL |
| **Architecture** | Clean Architecture + CQRS |
| **Mediator Pattern** | MediatR |
| **Validation** | FluentValidation |
| **Authentication** | JWT Bearer Tokens |
| **Design Pattern** | Repository Pattern |
| **Dependency Injection** | Built-in .NET DI |
| **Logging** | Microsoft.Extensions.Logging |

---

## 📂 Project Structure

```
backend/
│
├── Wallet.Api/                 # Presentation layer
│   ├── Controllers/
│   ├── DTOs/
│   ├── Middleware/
│   ├── Program.cs
│   └── appsettings.json
│
├── Wallet.Application/         # Application layer (CQRS)
│   ├── Common/                 # Interfaces, Behaviors, Exceptions
│   ├── Users/                  # User commands & queries
│   ├── Wallets/                # Wallet use cases
│   ├── Transactions/           # Transaction logic
│   ├── Security/               # Auth (Login, Logout)
│   └── DependencyInjection.cs
│
├── Wallet.Domain/              # Domain layer (Entities, Enums, ValueObjects)
│   ├── Entities/
│   ├── Enums/
│   ├── ValueObjects/
│   ├── Events/
│   └── Exceptions/
│
├── Wallet.Infrastructure/      # Infrastructure (Persistence, JWT, Repositories)
│   ├── Persistence/
│   │   ├── Configurations/
│   │   ├── Migrations/
│   │   └── Repositories/
│   ├── Security/
│   └── DependencyInjection.cs
│
└── Wallet.sln
```

---

## 🧰 Core Features

### 👤 **User Management**
- Register, Login, Logout, Refresh Token  
- Activate / Freeze / Disable / Close accounts  
- Fetch user details and dashboard data  

### 💰 **Wallet Management**
- Create wallet for user  
- Get wallet info and balance  
- Freeze or close wallet  

### 💸 **Transactions**
- Top-up wallet balance  
- Make and refund payments  
- Get transaction history (filter by type, status, or date)  

### 📊 **Dashboard**
- User financial overview  
- Recent transactions summary  
- Transaction counts by status  

---

## 🚀 Getting Started

### 1️⃣ Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- SQL Server or PostgreSQL
- Visual Studio / VS Code

---

### 2️⃣ Clone the Repository
```bash
git clone https://github.com/MohamedAftah004/wallet-system-clean-architecture.git
cd wallet-system/backend
```

---

### 3️⃣ Setup Database

Apply migrations and update the database:
```bash
dotnet ef database update
```

Ensure your **connection string** in `appsettings.json` points to your local DB:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=WalletDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

---

### 4️⃣ Run the API

```bash
dotnet run --project Wallet.Api
```

API will be available at:  
👉 https://localhost:7124  
or  
👉 http://localhost:5000  

---

### 5️⃣ Test with Swagger

Visit  
📄 **https://localhost:7124/swagger/index.html**  
to explore and test all endpoints interactively.

---

## 🧾 Example Endpoints

### 🔐 Authentication
**POST** `/api/auth/login`
```json
{
  "email": "user@example.com",
  "password": "Pass@123"
}
```
**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

---

### 💸 Top-Up Wallet
**POST** `/api/transactions/topup`
```json
{
  "walletId": "7c7557c1-bd66-4778-a722-0c2da72a8549",
  "amount": 150,
  "description": "Added balance for purchases"
}
```

---

### 📊 Get User Dashboard
**GET** `/api/users/{userId}/dashboard`
**Response:**
```json
{
  "userId": "b5a8f202-5e0b-4b7d-8e6c-4fefbcb8c7ef",
  "fullName": "Mohamed Aftah",
  "walletBalance": 460,
  "totalTransactions": 12,
  "completedTransactions": 9,
  "pendingTransactions": 3,
  "recentTransactions": [ ... ]
}
```

---

## 🧠 Design Highlights

- 🧩 **CQRS + MediatR** — Command & Query separation for scalability  
- 🗂️ **Repository Pattern** — Abstracts EF Core data access  
- ⚙️ **FluentValidation** — For input validation  
- 🚨 **Custom Middleware** — Global exception handling  
- 🔐 **JWT Authentication** — Secure, stateless access control  
- 🧱 **DTO Mapping** — Lightweight custom mapping (no AutoMapper overhead)

---

## 👨‍💻 Author

**Mohamed Aftah**  
Backend & Frontend Developer — (.NET / Angular / Clean Architecture)  

📧 [mohamedaftah04@gmail.com](mailto:mohamedaftah04@gmail.com)  
🔗 [GitHub](https://github.com/MohamedAftah004)  
🔗 [LinkedIn](https://www.linkedin.com/in/mabd-elfattah/)
