using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;
using Wallet.Infrastructure.Persistence;
using Wallet.Infrastructure.Persistence.Repositories;
using Xunit;

namespace Wallet.Tests.Infrastructure.Persistence.Repositories
{
    public class TransactionRepositoryTests
    {
        private AppDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private Wallet.Domain.Entities.Wallet CreateWallet(Guid? userId = null)
        {
            return new Wallet.Domain.Entities.Wallet(
                userId ?? Guid.NewGuid(),
                Currency.FromCode("USD")
            );
        }

        private Transaction CreateTransaction(Guid walletId)
        {
            return new Transaction(
                walletId,
                Money.Create(100, Currency.FromCode("USD")),
                TransactionType.TopUp,
                null,
                "Test Transaction"
            );
        }

      
        [Fact]
        public async Task AddAsync_Should_Add_Transaction()
        {
            var context = CreateInMemoryContext();
            var repo = new TransactionRepository(context);

            var wallet = CreateWallet();
            await context.Wallets.AddAsync(wallet);

            var transaction = CreateTransaction(wallet.Id);

            await repo.AddAsync(transaction);

            var saved = await context.Transactions.FirstOrDefaultAsync();

            Assert.NotNull(saved);
            Assert.Equal(wallet.Id, saved.WalletId);
        }

      
        [Fact]
        public async Task GetByIdAsync_Should_Return_Transaction_When_Found()
        {
            var context = CreateInMemoryContext();
            var repo = new TransactionRepository(context);

            var wallet = CreateWallet();
            var transaction = CreateTransaction(wallet.Id);

            await context.Wallets.AddAsync(wallet);
            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();

            var result = await repo.GetByIdAsync(transaction.Id);

            Assert.NotNull(result);
            Assert.Equal(transaction.Description, result.Description);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
        {
            var context = CreateInMemoryContext();
            var repo = new TransactionRepository(context);

            var result = await repo.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

       
        [Fact]
        public async Task GetByWalletIdAsync_Should_Return_All_Transactions()
        {
            var context = CreateInMemoryContext();
            var repo = new TransactionRepository(context);

            var wallet = CreateWallet();
            await context.Wallets.AddAsync(wallet);

            var t1 = CreateTransaction(wallet.Id);
            var t2 = CreateTransaction(wallet.Id);

            await context.Transactions.AddRangeAsync(t1, t2);
            await context.SaveChangesAsync();

            var result = await repo.GetByWalletIdAsync(wallet.Id);

            Assert.NotNull(result);
            Assert.Equal(2, result.AsList().Count);
        }

        
      
        [Fact]
        public void Query_Should_Return_IQueryable()
        {
            var context = CreateInMemoryContext();
            var repo = new TransactionRepository(context);

            var query = repo.Query();

            Assert.NotNull(query);
            Assert.IsAssignableFrom<IQueryable<Transaction>>(query);
        }
    }

    // Helper extension — makes counting IEnumerable easy
    internal static class EnumerableExtensions
    {
        public static List<T> AsList<T>(this IEnumerable<T> source)
            => new List<T>(source);
    }
}
