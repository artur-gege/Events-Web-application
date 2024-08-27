using FluentValidation;
using ModsenAPI.Domain.ModelsDTO.UserDTO;

namespace ModsenAPI.Application.Validators
{
    public class UserLoginValidator : AbstractValidator<UserLoginRequestDto>
    {
        public UserLoginValidator()
        {
            RuleFor(user => user.UserName).NotEmpty().WithMessage("Имя пользователя не может быть пустым.");
            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Пароль не может быть пустым.");
        }
    }
}
