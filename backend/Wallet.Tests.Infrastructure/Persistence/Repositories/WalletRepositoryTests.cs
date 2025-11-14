using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Wallet.Domain.ValueObjects;
using Wallet.Infrastructure.Persistence;
using Wallet.Infrastructure.Persistence.Repositories;
using WalletEntity = Wallet.Domain.Entities.Wallet;
using Xunit;

namespace Wallet.Tests.Infrastructure.Persistence.Repositories
{
    public class WalletRepositoryTests
    {
        private AppDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private WalletEntity CreateWallet(Guid? userId = null)
        {
            return new WalletEntity(
                userId ?? Guid.NewGuid(),
                Currency.FromCode("USD")
            );
        }

        
        [Fact]
        public async Task AddAsync_Should_Add_Wallet()
        {
            var context = CreateInMemoryContext();
            var repo = new WalletRepository(context);

            var wallet = CreateWallet();

            await repo.AddAsync(wallet);

            var saved = await context.Wallets.FirstOrDefaultAsync();

            Assert.NotNull(saved);
            Assert.Equal(wallet.UserId, saved.UserId);
        }

       
        [Fact]
        public async Task GetByUserIdAsync_Should_Return_Wallets()
        {
            var context = CreateInMemoryContext();
            var repo = new WalletRepository(context);

            var userId = Guid.NewGuid();

            var w1 = CreateWallet(userId);
            var w2 = CreateWallet(userId);
            var w3 = CreateWallet(Guid.NewGuid()); // different user

            await context.Wallets.AddRangeAsync(w1, w2, w3);
            await context.SaveChangesAsync();

            var result = await repo.GetByUserIdAsync(userId);

            Assert.Equal(2, result.Count());
            Assert.All(result, w => Assert.Equal(userId, w.UserId));
        }

      
        [Fact]
        public async Task GetByIdAsync_Should_Return_Wallet_When_Found()
        {
            var context = CreateInMemoryContext();
            var repo = new WalletRepository(context);

            var wallet = CreateWallet();

            await context.Wallets.AddAsync(wallet);
            await context.SaveChangesAsync();

            var result = await repo.GetByIdAsync(wallet.Id);

            Assert.NotNull(result);
            Assert.Equal(wallet.Id, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
        {
            var context = CreateInMemoryContext();
            var repo = new WalletRepository(context);

            var result = await repo.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_When_Id_Is_Empty()
        {
            var context = CreateInMemoryContext();
            var repo = new WalletRepository(context);

            var result = await repo.GetByIdAsync(Guid.Empty);

            Assert.Null(result);
        }

      
    }
}
