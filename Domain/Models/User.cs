using ModsenAPI.Domain.Enums;

namespace ModsenAPI.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
    }
}
