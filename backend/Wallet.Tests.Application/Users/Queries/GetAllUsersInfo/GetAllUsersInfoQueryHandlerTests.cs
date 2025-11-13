using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Common.Models;
using Wallet.Application.Users.DTOs;
using Wallet.Application.Users.Queries.GetAllUsersInfo;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Wallet.Tests.Application.Users.Queries.GetAllUsersInfo
{
    public class GetAllUsersInfoQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly GetAllUsersInfoQueryHandler _handler;

        public GetAllUsersInfoQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new GetAllUsersInfoQueryHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_PagedResult_With_Users()
        {
            // Arrange
            var users = new List<User>
            {
                new User("Mohamed Aftah", "mohamed@gmail.com", "01000000000", "hashedPass"),
                new User("Ali Ahmed", "ali@gmail.com", "01111111111", "hashedPass")
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<User>>();
            var queryable = new TestAsyncEnumerable<User>(users);

            _userRepositoryMock
                .Setup(r => r.Query())
                .Returns(users);

            var query = new GetAllUsersInfoQuery { Page = 1, Size = 10 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
            result.Items.First().FullName.Should().Be("Ali Ahmed");
            result.TotalCount.Should().Be(2);
        }

        private class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
        {
            public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }
            public TestAsyncEnumerable(Expression expression) : base(expression) { }
            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
            }
        }

        private class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _inner;
            public TestAsyncEnumerator(IEnumerator<T> inner) => _inner = inner;
            public T Current => _inner.Current;
            public ValueTask DisposeAsync() { _inner.Dispose(); return default; }
            public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext());
        }
    }
}
