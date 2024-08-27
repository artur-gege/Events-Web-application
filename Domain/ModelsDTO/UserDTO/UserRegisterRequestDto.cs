using ModsenAPI.Domain.Enums;

namespace ModsenAPI.Domain.ModelsDTO.UserDTO
{
    public class UserRegisterRequestDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}
