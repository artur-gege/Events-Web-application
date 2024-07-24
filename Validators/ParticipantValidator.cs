using FluentValidation;
using ModsenAPI.ModelsDTO;

namespace ModsenAPI.Validators
{
    public class ParticipantValidator : AbstractValidator<ParticipantDto>
    {
        public ParticipantValidator()
        {
            RuleFor(participant => participant.Name).NotEmpty().WithMessage("Имя участника не может быть пустым.");
            RuleFor(participant => participant.Surname).NotEmpty().WithMessage("Фамилия участника не может быть пустой.");
            RuleFor(participant => participant.DateOfBirth).LessThan(DateTime.Now).WithMessage("Дата рождения участника должна быть в прошлом.");
            RuleFor(participant => participant.RegistrationDate).LessThanOrEqualTo(DateTime.Now).WithMessage("Дата регистрации участника не может быть в будущем.");
            RuleFor(participant => participant.Email).EmailAddress().WithMessage("Пожалуйста, введите корректный email.");
        }
    }
}
