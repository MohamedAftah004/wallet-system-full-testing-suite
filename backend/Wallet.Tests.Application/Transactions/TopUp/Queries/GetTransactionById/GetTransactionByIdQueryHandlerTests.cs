using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Transactions.TopUp.DTOs;
using Wallet.Application.Transactions.TopUp.Queries.GetTransactionById;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;
using Xunit;

namespace Wallet.Tests.Application.Transactions.TopUp.Queries.GetTransactionById
{
    public class GetTransactionByIdQueryHandlerTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly GetTransactionByIdQueryHandler _handler;

        public GetTransactionByIdQueryHandlerTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _handler = new GetTransactionByIdQueryHandler(_transactionRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFound_WhenTransactionNotFound()
        {
            // Arrange
            var query = new GetTransactionByIdQuery(Guid.NewGuid());
            _transactionRepositoryMock.Setup(x => x.GetByIdAsync(query.TransactionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Transaction?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }

    
    }
}
