using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Users.DTOs;
using Wallet.Application.Users.Queries.GetUserInfoById;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Xunit;

namespace Wallet.Tests.Application.Users.Queries.GetUserInfoById
{
    public class GetUserInfoByIdQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly GetUserInfoByIdQueryHandler _handler;

        public GetUserInfoByIdQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new GetUserInfoByIdQueryHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnUserDto_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User("mohamed aftah", "mohamed@gmail.com", "+201000000000", "hashed123");
            user.Activate();

            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var query = new GetUserInfoByIdQuery(userId);

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
            var userId = Guid.NewGuid();

            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);

            var query = new GetUserInfoByIdQuery(userId);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}
