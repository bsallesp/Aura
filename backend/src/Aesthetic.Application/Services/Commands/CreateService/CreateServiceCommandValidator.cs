using FluentValidation;

namespace Aesthetic.Application.Services.Commands.CreateService;

public class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
{
    public CreateServiceCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Service name is required.")
            .MaximumLength(100).WithMessage("Service name must not exceed 100 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0).WithMessage("Duration must be greater than zero.")
            .LessThanOrEqualTo(480).WithMessage("Duration cannot exceed 8 hours (480 minutes).");
    }
}
