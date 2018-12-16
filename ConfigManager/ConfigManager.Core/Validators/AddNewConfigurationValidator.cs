using ConfigManager.Core.Const;
using ConfigManager.Core.DTOs;
using FluentValidation;

namespace ConfigManager.Core.Validators
{
    public class AddNewConfigurationValidator : AbstractValidator<AddConfigurationDTO>
    {
        public AddNewConfigurationValidator()
        {
            RuleFor(p => p).NotNull().WithMessage(ErrorMessage.MissingInformation);
            When(p => p != null, () =>
            {
                RuleFor(p => p.Name).Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage(ErrorMessage.NameCanNotBeEmpty);
                RuleFor(p => p.Type).Must(type => !string.IsNullOrWhiteSpace(type)).WithMessage(ErrorMessage.TypeCanNotBeEmpty);
                RuleFor(p => p.Value).Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage(ErrorMessage.ValueCanNotBeEmpty);
            });
        }
    }
}
