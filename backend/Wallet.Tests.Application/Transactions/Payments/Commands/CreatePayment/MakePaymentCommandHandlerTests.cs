using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Common.Services;
using Wallet.Application.Transactions.Payments.Commands.MakePayment;
using Wallet.Application.Transactions.Payments.DTOs;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;
using Xunit;

namespace Wallet.Tests.Application.Transactions.Payments.Commands.MakePayment
{
    public class MakePaymentCommandHandlerTests
    {
        private readonly Mock<IWalletRepository> _walletRepositoryMock;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IUserValidator> _userValidatorMock;
        private readonly MakePaymentCommandHandler _handler;

        public MakePaymentCommandHandlerTests()
        {
            _walletRepositoryMock = new Mock<IWalletRepository>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _userValidatorMock = new Mock<IUserValidator>();

            _handler = new MakePaymentCommandHandler(
                _walletRepositoryMock.Object,
                _transactionRepositoryMock.Object,
                _userValidatorMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenWalletNotFound()
        {
            // Arrange
            var command = new MakePaymentCommand(Guid.NewGuid(), 100, "Test Payment");
            _walletRepositoryMock.Setup(x => x.GetByIdAsync(command.WalletId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Wallet.Domain.Entities.Wallet?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenWalletNotActive()
        {
            // Arrange
            var wallet = new Wallet.Domain.Entities.Wallet(Guid.NewGuid(), Currency.FromCode("USD"));


            var command = new MakePaymentCommand(wallet.Id, 50, "Inactive Wallet Payment");

            _walletRepositoryMock.Setup(x => x.GetByIdAsync(wallet.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(wallet);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Only active wallets can make payments.", ex.Message);
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenInsufficientBalance()
        {
            // Arrange
            var wallet = new Wallet.Domain.Entities.Wallet(Guid.NewGuid(), Currency.FromCode("USD"));
            wallet.Activate();
            wallet.Deduct(50);

            var command = new MakePaymentCommand(wallet.Id, 100, "Insufficient funds");

            _walletRepositoryMock.Setup(x => x.GetByIdAsync(wallet.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(wallet);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Insufficient balance.", ex.Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnPaymentResponse_WhenPaymentIsSuccessful()
        {
            // Arrange
            var wallet = new Wallet.Domain.Entities.Wallet(Guid.NewGuid(), Currency.FromCode("USD"));
            wallet.Activate();
            wallet.Deduct(500);

            var command = new MakePaymentCommand(wallet.Id, 100, "Test successful payment");

            _walletRepositoryMock.Setup(x => x.GetByIdAsync(wallet.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(wallet);

            _userValidatorMock.Setup(x => x.EnsureUserIsActiveAsync(wallet.UserId, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _transactionRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _walletRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Wallet.Domain.Entities.Wallet>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PaymentResponseDto>(result);
            Assert.Equal(wallet.Id, result.WalletId);
            Assert.Equal(100, result.Amount);
            Assert.Equal("EGP", result.CurrencyCode);
            Assert.Equal("Completed", result.Status);
            Assert.Equal("Test successful payment", result.Description);
        }
    }
}