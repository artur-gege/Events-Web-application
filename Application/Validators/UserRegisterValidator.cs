using FluentValidation;
using ModsenAPI.Domain.ModelsDTO.ParticipantDTO;
using ModsenAPI.Domain.ModelsDTO.UserDTO;

namespace ModsenAPI.Application.Validators
{
    public class UserRegisterValidator : AbstractValidator<UserRegisterRequestDto>
    {
        public UserRegisterValidator()
        {
            RuleFor(user => user.UserName).NotEmpty().WithMessage("Имя пользователя не может быть пустым.");
            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Пароль не может быть пустым.");
            RuleFor(user => user.Role).IsInEnum().WithMessage("Некорректная роль пользователя.");
        }
    }

}
