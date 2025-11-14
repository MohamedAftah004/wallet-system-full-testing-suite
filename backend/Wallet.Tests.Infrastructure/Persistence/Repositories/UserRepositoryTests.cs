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
    public class UserRepositoryTests
    {
        private AppDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private User CreateTestUser()
        {
            return new User("TestUser", "test@example.com", "111222333", "hashedPwd");
        }

        [Fact]
        public async Task AddAsync_Should_Add_User()
        {
            var context = CreateInMemoryContext();
            var repo = new UserRepository(context);

            var user = CreateTestUser();

            await repo.AddAsync(user);

            var saved = await context.Users.FirstOrDefaultAsync();

            Assert.NotNull(saved);
            Assert.Equal("TestUser", saved.FullName);
        }

        
        [Fact]
        public async Task GetByIdAsync_Should_Return_User_When_Found()
        {
            var context = CreateInMemoryContext();
            var repo = new UserRepository(context);

            var user = CreateTestUser();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var result = await repo.GetByIdAsync(user.Id);

            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
        {
            var context = CreateInMemoryContext();
            var repo = new UserRepository(context);

            var result = await repo.GetByIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        
        [Fact]
        public async Task GetByEmailAsync_Should_Return_User_When_Email_Exists()
        {
            var context = CreateInMemoryContext();
            var repo = new UserRepository(context);

            var user = CreateTestUser();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var result = await repo.GetByEmailAsync("test@example.com");

            Assert.NotNull(result);
            Assert.Equal("TestUser", result.FullName);
        }

        [Fact]
        public async Task GetByEmailAsync_Should_Return_Null_When_Email_Not_Found()
        {
            var context = CreateInMemoryContext();
            var repo = new UserRepository(context);

            var result = await repo.GetByEmailAsync("notfound@example.com");

            Assert.Null(result);
        }

        
        [Fact]
        public async Task ExistsByEmail_Should_Return_True_When_Email_Exists()
        {
            var context = CreateInMemoryContext();
            var repo = new UserRepository(context);

            var user = CreateTestUser();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var exists = await repo.ExsistByEmailAsync(user.Email);

            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsByPhone_Should_Return_False_When_Not_Found()
        {
            var context = CreateInMemoryContext();
            var repo = new UserRepository(context);

            var exists = await repo.ExsistByPhoneAsync("000");

            Assert.False(exists);
        }

       
        [Fact]
        public async Task Activate_Should_Update_User_Status()
        {
            var context = CreateInMemoryContext();
            var repo = new UserRepository(context);

            var user = CreateTestUser();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            await repo.ActivateAsync(user.Id);

            var updated = await context.Users.FirstAsync();

            Assert.Equal(UserStatus.Active, updated.UserStatus);
        }

        [Fact]
        public async Task Disable_Should_Update_User_Status()
        {
            var context = CreateInMemoryContext();
            var repo = new UserRepository(context);

            var user = CreateTestUser();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            await repo.DisableAsync(user.Id);

            var updated = await context.Users.FirstAsync();

            Assert.Equal(UserStatus.Disabled, updated.UserStatus);
        }

        [Fact]
        public async Task Freeze_Should_Update_User_Status()
        {
            var context = CreateInMemoryContext();
            var repo = new UserRepository(context);

            var user = CreateTestUser();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            await repo.FreezeAsync(user.Id);

            var updated = await context.Users.FirstAsync();

            Assert.Equal(UserStatus.Frozen, updated.UserStatus);
        }

        [Fact]
        public async Task Close_Should_Update_User_Status()
        {
            var context = CreateInMemoryContext();
            var repo = new UserRepository(context);

            var user = CreateTestUser();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            await repo.CloseAsync(user.Id);

            var updated = await context.Users.FirstAsync();

            Assert.Equal(UserStatus.Closed, updated.UserStatus);
        }
    }
}
