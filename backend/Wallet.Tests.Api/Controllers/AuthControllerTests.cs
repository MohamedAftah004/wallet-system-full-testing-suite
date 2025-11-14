using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Wallet.Tests.Api.Setup;
using Wallet.Application.Security.Commands.Login;
using Wallet.Application.Security.DTOs;
using Xunit;
using Moq;

namespace Wallet.Tests.Api.Controllers
{
    public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public AuthControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Login_Should_Return_200_And_AuthResult()
        {
            // Arrange
            var request = new LoginCommand("test@example.com", "123456");

            var fakeResult = new AuthResultDto
            {
                UserId = Guid.NewGuid(),
                Email = "test@example.com",
                AccessToken = "FakeJWTToken",
                RefreshToken = "FakeRefreshToken",
                Status = "Active"
            };

            _factory.MediatorMock
                .Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeResult);

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var body = await response.Content.ReadFromJsonAsync<AuthResultDto>();

            body.Should().NotBeNull();
            body!.Email.Should().Be("test@example.com");
            body.AccessToken.Should().Be("FakeJWTToken");
        }
    }
}
