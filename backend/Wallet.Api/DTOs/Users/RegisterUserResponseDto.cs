using Wallet.Domain.Enums;

namespace Wallet.Api.DTOs.Users
{
    public class RegisterUserResponseDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public UserStatus Status { get; set; } = UserStatus.PendingActivation;
    }
}
