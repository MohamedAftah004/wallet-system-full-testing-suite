using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Users.DTOs;
using Wallet.Application.Users.Queries.GetUserInfoByEmail;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Xunit;

namespace Wallet.Tests.Application.Users.Queries.GetUserInfoByEmail
{
    public class GetUserInfoByEmailQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly GetUserInfoByEmailQueryHandler _handler;

        public GetUserInfoByEmailQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new GetUserInfoByEmailQueryHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnUserDto_WhenUserExists()
        {
            // Arrange
            var email = "mohamed@gmail.com";
            var user = new User("Mohamed Aftah", email, "+201000000000", "hashed123");
            user.Activate(); // نخلي الحالة Active

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var query = new GetUserInfoByEmailQuery(email);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UserDto>(result);
            Assert.Equal(user.Id, result.UserId);
            Assert.Equal(user.FullName, result.FullName);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.PhoneNumber, result.PhoneNumber);
            Assert.Equal(user.UserStatus.ToString(), result.Status);
        }

        [Fact]
        public async Task Handle_ShouldThrowEntityNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var email = "notfound@gmail.com";
            _userRepositoryMock.Setup(x => x.GetByEmailAsync(email, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            var query = new GetUserInfoByEmailQuery(email);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}
