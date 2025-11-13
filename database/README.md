# 🗄️ Wallet Database — PostgreSQL 17

The **WalletDb** is a **PostgreSQL 17** database that stores all core data for the Wallet System — including **users**, **wallets**, and **transactions**.  
It’s exported as a single backup file `WalletDb.backup`, ready to restore via **pgAdmin** or **pg_restore**.

---

## ⚙️ Overview

- 🧍 Users → Accounts and authentication info  
- 💰 Wallets → User balances with currency codes  
- 💸 Transactions → Top-ups, payments, refunds  
- 🔗 Relationships with cascade delete  
- ⚙️ EFCore migrations support  

---

## 🧱 Schema Summary

| Table         | Description                          |
| --------------| ------------------------------------ |
| **Users**     | User details (email, name, status)   |
| **Wallets**   | Linked to users, store balance       |
| **Transactions** | Linked to wallets, track payments |
| **__EFMigrationsHistory** | Migration tracking       |

**Relations:**  
`Users (1) ───< Wallets (∞) ───< Transactions (∞)`

---

## 🚀 Restore Database

### 🧩 Option 1 — Using pgAdmin  
1. Create database `WalletDb`  
2. Right-click → **Restore...**  
3. Choose `WalletDb.backup` → Format: *Custom/Tar*  
4. ✅ Click **Restore**

### 🧩 Option 2 — Using CLI  
```bash
createdb -U postgres WalletDb
pg_restore -U postgres -d WalletDb "WalletDb.backup"
```

---

## 🧠 Notes
- Generated via `pg_dump v17.6`  
- Compatible with **PostgreSQL 15+**  
- Matches entities in `.NET 9 EFCore` backend  
- Includes sample users, wallets, and transactions  

---

👨‍💻 **Author**  
**Mohamed Aftah** — Full Stack Developer (.NET | Angular | PostgreSQL)  
📧 mohamedaftah04@gmail.com  
🔗 [GitHub](https://github.com/MohamedAftah004)  
🔗 [LinkedIn](https://www.linkedin.com/in/mabd-elfattah/)
