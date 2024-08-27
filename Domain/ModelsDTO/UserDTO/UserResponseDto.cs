using ModsenAPI.Domain.Enums;

namespace ModsenAPI.Domain.ModelsDTO.UserDTO
{
    public class UserResponseDto
    {
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}
