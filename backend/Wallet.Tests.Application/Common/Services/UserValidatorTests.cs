using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Common.Services;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Xunit;

namespace Wallet.Tests.Application.Common.NewFolder
{
    public class UserValidatorTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserValidator _userValiadtor;

        public UserValidatorTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userValiadtor = new UserValidator(_userRepositoryMock.Object);
        }


        [Fact]
        public async Task EnsureUserIsActiveAsync_Should_Throw_KeyNotFoundException_When_User_Not_Found()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _userRepositoryMock
                .Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as User);

            // Act
            var act = async () => await _userValiadtor.EnsureUserIsActiveAsync(userId, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"*{userId}*");
        }


    }
}
