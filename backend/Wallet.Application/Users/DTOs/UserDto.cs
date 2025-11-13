namespace Wallet.Application.Users.DTOs
{
    /// <summary>
    /// Data Transfer Object representing basic user information
    /// returned from queries such as GetUserInfo.
    /// </summary>
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
