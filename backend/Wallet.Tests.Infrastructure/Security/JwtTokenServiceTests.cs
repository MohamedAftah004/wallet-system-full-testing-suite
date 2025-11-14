using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Infrastructure.Security;
using Xunit;

namespace Wallet.Tests.Infrastructure.Security
{
    public class JwtTokenServiceTests
    {
        private IJwtTokenService CreateService()
        {
            var inMemorySettings = new Dictionary<string, string?>
            {
                { "JwtSettings:Key", "MySuperStrongSecretKeyMohamed2025Wallet" },
                { "JwtSettings:Issuer", "WalletApi" },
                { "JwtSettings:Audience", "WalletApiUsers" },
                { "JwtSettings:AccessTokenExpirationMinutes", "30" }
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            return new JwtTokenService(configuration);
        }

        private User CreateUser()
        {
            return new User(
                fullName: "Test User",
                email: "test@example.com",
                phoneNumber: "123456789",
                passwordHash: "hashed123"
            );
        }

        [Fact]
        public void GenerateAccessToken_Should_Return_Valid_JWT()
        {
            var service = CreateService();
            var user = CreateUser();

            var tokenString = service.GenerateAccessToken(user);
            Assert.False(string.IsNullOrWhiteSpace(tokenString));

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);

            Assert.Equal(user.Id.ToString(), token.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value);
            Assert.Equal(user.Email, token.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value);
            Assert.Equal(user.FullName, token.Claims.First(c => c.Type == ClaimTypes.Name).Value);

            Assert.Equal("WalletApi", token.Issuer);
            Assert.Contains("WalletApiUsers", token.Audiences);
        }

        [Fact]
        public void GenerateAccessToken_Should_Be_Signed_Correctly()
        {
            var service = CreateService();
            var user = CreateUser();

            var tokenString = service.GenerateAccessToken(user);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);

            Assert.NotNull(token.SignatureAlgorithm);
            Assert.Equal(SecurityAlgorithms.HmacSha256, token.SignatureAlgorithm);
        }

        [Fact]
        public void GenerateRefreshToken_Should_Return_Base64_String()
        {
            var service = CreateService();

            var token = service.GenerateRefreshToken();

            bool isBase64 = Convert.TryFromBase64String(token, new byte[token.Length], out _);

            Assert.True(isBase64);
        }

        [Fact]
        public void GenerateRefreshToken_Should_Return_Unique_Tokens()
        {
            var service = CreateService();

            var t1 = service.GenerateRefreshToken();
            var t2 = service.GenerateRefreshToken();

            Assert.NotEqual(t1, t2);
        }

        [Fact]
        public void GenerateRefreshToken_Should_Have_Minimum_Length()
        {
            var service = CreateService();

            var token = service.GenerateRefreshToken();

            Assert.True(token.Length >= 80);
        }
    }
}
