using Microsoft.EntityFrameworkCore;
using System;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;
using Wallet.Infrastructure.Persistence;
using CurrentWallet = Wallet.Domain.Entities.Wallet;
using Xunit;

namespace Wallet.Tests.Infrastructure.Persistence
{
    public class AppDbContextTests
    {
        private AppDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public void DbContext_ShouldInitialize()
        {
            // Act
            var context = CreateInMemoryContext();

            // Assert
            Assert.NotNull(context);
        }

        [Fact]
        public void DbSets_ShouldExist()
        {
            var context = CreateInMemoryContext();

            Assert.NotNull(context.Users);
            Assert.NotNull(context.Wallets);
            Assert.NotNull(context.Transactions);
        }

        [Fact]
        public async Task Should_Add_And_Retrieve_User()
        {
            var context = CreateInMemoryContext();

            var user = new User("TestUser", "test@example.com", "123456" , "passwordHashedd");

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var savedUser = await context.Users.FirstOrDefaultAsync(x => x.Email == "test@example.com");

            Assert.NotNull(savedUser);
            Assert.Equal("TestUser", savedUser.FullName);
        }

        [Fact]
        public async Task Should_Add_Wallet_And_Link_To_User()
        {
            var context = CreateInMemoryContext();

            var user = new User("TestUser", "test@example.com", "123456", "passwordHashedd");
            await context.Users.AddAsync(user);


            var wallet = new CurrentWallet(user.Id , Currency.FromCode("USD"));
            await context.Wallets.AddAsync(wallet);

            await context.SaveChangesAsync();

            var savedWallet = await context.Wallets.FirstAsync();

            Assert.NotNull(savedWallet);
            Assert.Equal(user.Id, savedWallet.UserId);
        }

        [Fact]
        public async Task Should_Add_Transaction_And_Link_To_Wallet()
        {
            var context = CreateInMemoryContext();

            var user = new User("TestUser", "test@example.com", "123456", "passwordHashedd");
            await context.Users.AddAsync(user);

            var wallet = new CurrentWallet(user.Id, Currency.FromCode("USD"));
            await context.Wallets.AddAsync(wallet);

            var transaction = new Transaction(
                wallet.Id,
                Money.Create(100, Currency.FromCode("EGP")),
                TransactionType.TopUp,
                null,
                "Test Transaction"
            );

            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();

            var saved = await context.Transactions.FirstAsync();

            Assert.NotNull(saved);
            Assert.Equal(wallet.Id, saved.WalletId);
            Assert.Equal(100, saved.Amount.Amount);
        }

        [Fact]
        public void OnModelCreating_ShouldApplyConfigurations()
        {
            var context = CreateInMemoryContext();

            var model = context.Model;

            var walletEntity = model.FindEntityType(typeof(Wallet.Domain.Entities.Wallet));
            var transactionEntity = model.FindEntityType(typeof(Transaction));

            Assert.NotNull(walletEntity);
            Assert.NotNull(transactionEntity);
        }
    }
}
