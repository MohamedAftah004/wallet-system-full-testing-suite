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
