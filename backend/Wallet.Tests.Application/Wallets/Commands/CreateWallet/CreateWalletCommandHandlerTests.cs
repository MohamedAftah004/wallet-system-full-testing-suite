using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Wallets.Commands.CreateWallet;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;
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
        public async Task Handle_ShouldThrowException_WhenUserDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var command = new CreateWalletCommand(userId, "EGP");

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

       
        [Fact]
        public async Task Handle_ShouldCreateWallet_WhenDataIsValid()
        {
            var userId = Guid.NewGuid();
            var command = new CreateWalletCommand(userId, "EGP");

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new User("mohamed", "m@m.com", "+20100", "hash"));

            _walletRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<CurrentWallet>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal("EGP", result.Currency);

            _walletRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<CurrentWallet>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

    }
}
