using FluentValidation;
using ModsenAPI.ModelsDTO;

namespace ModsenAPI.Validators
{
    public class EventValidator : AbstractValidator<EventDto>
    {
        public EventValidator()
        {
            RuleFor(_event => _event.Name).NotEmpty().WithMessage("Название события не может быть пустым.");
            RuleFor(_event => _event.Description).NotEmpty().WithMessage("Описание события не может быть пустым.");
            RuleFor(_event => _event.DateTime).GreaterThan(DateTime.Now).WithMessage("Дата и время события должны быть в будущем.");
            RuleFor(_event => _event.Location).NotEmpty().WithMessage("Место проведения события не может быть пустым.");
            RuleFor(_event => _event.Category).NotEmpty().WithMessage("Категория события не может быть пустой.");
            RuleFor(_event => _event.MaxParticipants).GreaterThanOrEqualTo(1).WithMessage("Максимальное количество участников должно быть не менее 1.");
            RuleFor(_event => _event.ImageUrl).NotEmpty().WithMessage("URL изображения не может быть пустым.");
        }
    }
}
