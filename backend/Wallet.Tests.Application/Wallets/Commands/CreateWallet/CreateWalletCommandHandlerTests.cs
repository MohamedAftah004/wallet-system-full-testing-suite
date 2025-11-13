using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Wallets.Commands.CreateWallet;
using Xunit;
using CurrentWallet = Wallet.Domain.Entities.Wallet;

namespace Wallet.Tests.Application.Wallets.Commands.CreateWallet
{
    public class CreateWalletCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IWalletRepository> _walletRepositoryMock;
        private readonly CreateWalletCommandHandler _handler;

        public CreateWalletCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _walletRepositoryMock = new Mock<IWalletRepository>();

            _handler = new CreateWalletCommandHandler(
                _userRepositoryMock.Object,
                _walletRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldCreateWallet_WhenDataIsValid()
        {
            // Arrange
            var command = new CreateWalletCommand(Guid.NewGuid(), "EGP");

            _walletRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<CurrentWallet>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _walletRepositoryMock.Verify(x => x.AddAsync(It.IsAny<CurrentWallet>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
