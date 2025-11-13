using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Users.DTOs;
using Wallet.Application.Users.Queries.GetUserDashboard;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;
using Xunit;
using CurrentWallet = Wallet.Domain.Entities.Wallet;

namespace Wallet.Tests.Application.Users.Queries.GetUserDashboard
{
    public class GetUserDashboardQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IWalletRepository> _walletRepositoryMock;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly GetUserDashboardQueryHandler _handler;

        public GetUserDashboardQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _walletRepositoryMock = new Mock<IWalletRepository>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();

            _handler = new GetUserDashboardQueryHandler(
                _userRepositoryMock.Object,
                _walletRepositoryMock.Object,
                _transactionRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnUserDashboard_WhenDataExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User("mohamed aftah", "mohamed@gmail.com", "+201000000000", "hashed123");

            var wallet1 = new CurrentWallet(user.Id, Currency.FromCode("EGP"));
            wallet1.TopUp(1000);

            var wallet2 = new CurrentWallet(user.Id, Currency.FromCode("EGP"));
            wallet2.TopUp(500);

            var transactions = new List<Transaction>
            {
                new Transaction(wallet1.Id, Money.Create(200, Currency.FromCode("EGP")), TransactionType.TopUp, null, "TopUp done"),
                new Transaction(wallet1.Id, Money.Create(100, Currency.FromCode("EGP")), TransactionType.Transfer, null, "Pending transfer"),
                new Transaction(wallet2.Id, Money.Create(50, Currency.FromCode("EGP")), TransactionType.TopUp, null, "TopUp 2")
            };

            transactions[0].MarkCompleted();
            transactions[2].MarkCompleted();


            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _walletRepositoryMock.Setup(x => x.GetByUserIdAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CurrentWallet> { wallet1, wallet2 });

            _transactionRepositoryMock.Setup(x => x.Query())
                .Returns(transactions.AsQueryable());

            var query = new GetUserDashboardQuery(userId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.UserId);
            Assert.Equal("mohamed aftah", result.FullName);
            Assert.Equal(1500, result.WalletBalance);
            Assert.Equal(3, result.TotalTransactions);
            Assert.Equal(2, result.CompletedTransactions);
            Assert.Equal(1, result.PendingTransactions);
            Assert.True(result.RecentTransactions.Any());
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            var query = new GetUserDashboardQuery(userId);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUserHasNoWallets()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User("mohamed aftah", "mohamed@gmail.com", "+201000000000", "hashed123");

            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            _walletRepositoryMock.Setup(x => x.GetByUserIdAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CurrentWallet>()); 

            var query = new GetUserDashboardQuery(userId);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));
        }
    }
}
