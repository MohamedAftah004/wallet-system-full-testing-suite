using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using Wallet.Application.Common.Behaviors;
using Xunit;

namespace Wallet.Tests.Application.Common.Behaviors
{
    public class ValidationBehaviorTests
    {
        [Fact]
        public async Task Handle_ShouldThrowValidationException_WhenValidationFails()
        {
            // Arrange
            var request = new FakeRequest();
            var validatorMock = new Mock<IValidator<FakeRequest>>();

            validatorMock
                .Setup(v => v.Validate(It.IsAny<ValidationContext<FakeRequest>>()))
                .Returns(new FluentValidation.Results.ValidationResult(new[]
                {
                    new FluentValidation.Results.ValidationFailure("Name", "Name is required")
                }));

            var validators = new List<IValidator<FakeRequest>> { validatorMock.Object };
            var behavior = new ValidationBehavior<FakeRequest, FakeResponse>(validators);

            RequestHandlerDelegate<FakeResponse> next = async (cancellationToken) =>
            {
                await Task.Yield();
                return new FakeResponse();
            };

            // Act
            var act = async () => await behavior.Handle(request, next, default);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("*Name is required*");
        }

        [Fact]
        public async Task Handle_Should_Call_Next_When_No_Validators()
        {
            // Arrange
            var validators = new List<IValidator<FakeRequest>>();
            var behavior = new ValidationBehavior<FakeRequest, FakeResponse>(validators);
            var request = new FakeRequest();
            var expectedResponse = new FakeResponse();

            bool nextCalled = false;

            RequestHandlerDelegate<FakeResponse> next = async (CancellationToken _) =>
            {
                nextCalled = true;
                await Task.CompletedTask;
                return expectedResponse;
            };

            // Act
            var response = await behavior.Handle(request, next, CancellationToken.None);

            // Assert
            nextCalled.Should().BeTrue();
            response.Should().Be(expectedResponse);
        }

        [Fact]
        public async Task Handle_Should_Call_Next_When_Validation_Succeeds()
        {
            // Arrange
            var validatorMock = new Mock<IValidator<FakeRequest>>();
            validatorMock
                .Setup(v => v.Validate(It.IsAny<ValidationContext<FakeRequest>>()))
                .Returns(new FluentValidation.Results.ValidationResult()); // ✅ no errors

            var validators = new List<IValidator<FakeRequest>> { validatorMock.Object };
            var behavior = new ValidationBehavior<FakeRequest, FakeResponse>(validators);

            var request = new FakeRequest();
            var expectedResponse = new FakeResponse();

            bool nextCalled = false;

            RequestHandlerDelegate<FakeResponse> next = async (CancellationToken _) =>
            {
                nextCalled = true;
                await Task.CompletedTask;
                return expectedResponse;
            };

            // Act
            var response = await behavior.Handle(request, next, CancellationToken.None);

            // Assert
            nextCalled.Should().BeTrue();
            response.Should().Be(expectedResponse);
        }

        [Fact]
        public async Task Handle_Should_Run_All_Validators()
        {
            // Arrange
            var validator1 = new Mock<IValidator<FakeRequest>>();
            var validator2 = new Mock<IValidator<FakeRequest>>();

            validator1
                .Setup(v => v.Validate(It.IsAny<ValidationContext<FakeRequest>>()))
                .Returns(new FluentValidation.Results.ValidationResult());

            validator2
                .Setup(v => v.Validate(It.IsAny<ValidationContext<FakeRequest>>()))
                .Returns(new FluentValidation.Results.ValidationResult());

            var validators = new List<IValidator<FakeRequest>> { validator1.Object, validator2.Object };
            var behavior = new ValidationBehavior<FakeRequest, FakeResponse>(validators);

            var request = new FakeRequest();
            RequestHandlerDelegate<FakeResponse> next = async (CancellationToken _) =>
            {
                await Task.CompletedTask;
                return new FakeResponse();
            };

            // Act
            await behavior.Handle(request, next, CancellationToken.None);

            // Assert
            validator1.Verify(v => v.Validate(It.IsAny<ValidationContext<FakeRequest>>()), Times.Once);
            validator2.Verify(v => v.Validate(It.IsAny<ValidationContext<FakeRequest>>()), Times.Once);
        }

        public class FakeRequest : IRequest<FakeResponse> { }

        public class FakeResponse { }
    }
}
