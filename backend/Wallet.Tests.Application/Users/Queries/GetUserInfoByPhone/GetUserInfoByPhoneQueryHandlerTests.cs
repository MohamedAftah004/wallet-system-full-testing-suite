using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Users.DTOs;
using Wallet.Application.Users.Queries.GetUserInfoByPhone;
using Wallet.Domain.Entities;
using Xunit;

namespace Wallet.Tests.Application.Users.Queries.GetUserInfoByPhone
{
    public class GetUserInfoByPhoneQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly GetUserInfoByEmailQueryHandler _handler;

        public GetUserInfoByPhoneQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new GetUserInfoByEmailQueryHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnUserDto_WhenUserExists()
        {
            // Arrange
            var phone = "+201000000000";
            var user = new User("mohamed aftah", "mohamed@gmail.com", phone, "hashed123");

            _userRepositoryMock
                .Setup(x => x.GetByPhoneAsync(phone, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var query = new GetUserInfoByPhoneQuery(phone);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.UserId);
            Assert.Equal("mohamed aftah", result.FullName);
            Assert.Equal("mohamed@gmail.com", result.Email);
            Assert.Equal(phone, result.PhoneNumber);
            Assert.Equal(user.UserStatus.ToString(), result.Status);
        }

        [Fact]
        public async Task Handle_ShouldThrowEntityNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var phone = "+201000000000";

            _userRepositoryMock
                .Setup(x => x.GetByPhoneAsync(phone, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            var query = new GetUserInfoByPhoneQuery(phone);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}
