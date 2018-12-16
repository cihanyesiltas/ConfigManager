using ConfigManager.Core.Const;
using ConfigManager.Core.DTOs;
using FluentValidation;

namespace ConfigManager.Core.Validators
{
    public class UpdateConfigurationValidator : AbstractValidator<UpdateConfigurationDTO>
    {
        public UpdateConfigurationValidator()
        {
            RuleFor(p => p).NotNull().WithMessage(ErrorMessage.MissingInformation);
            When(p => p != null, () =>
            {
                RuleFor(p => p.Id).Must(id => !string.IsNullOrWhiteSpace(id)).WithMessage(ErrorMessage.IdCanNotBeEmpty);
                RuleFor(p => p.Type).Must(type => !string.IsNullOrWhiteSpace(type)).WithMessage(ErrorMessage.TypeCanNotBeEmpty);
                RuleFor(p => p.Value).Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage(ErrorMessage.ValueCanNotBeEmpty);
            });
        }
    }
}
