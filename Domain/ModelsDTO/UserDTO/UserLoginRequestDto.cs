namespace ModsenAPI.Domain.ModelsDTO.UserDTO
{
    public class UserLoginRequestDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
