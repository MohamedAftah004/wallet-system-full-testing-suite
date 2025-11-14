using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Moq;
using Wallet.Application.Transactions.TopUp.Commands.TopUpWallet;
using Wallet.Tests.Api.Setup;
using Xunit;

namespace Wallet.Tests.Api.Controllers
{
    public class TransactionsControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public TransactionsControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task TopUp_Should_Return_200_And_Result()
        {
            // Arrange
            var command = new TopUpWalletCommand(
                WalletId: Guid.NewGuid(),
                Amount: 150,
                Description: "Test topup"
            );

            var fakeResult = new
            {
                TransactionId = Guid.NewGuid(),
                Amount = 150,
                Message = "Top-up successful"
            };

            _factory.MediatorMock
     .Setup(m => m.Send<object>(It.IsAny<TopUpWalletCommand>(), It.IsAny<CancellationToken>()))
     .ReturnsAsync(fakeResult);




            // Act
            var response = await _client.PostAsJsonAsync("/api/transactions/topup", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await response.Content.ReadFromJsonAsync<dynamic>();
            ((int)body.Amount).Should().Be(150);
        }
    }
}
