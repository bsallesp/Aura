using FluentValidation;

namespace Aesthetic.Application.Appointments.Commands.BookAppointment;

public class BookAppointmentCommandValidator : AbstractValidator<BookAppointmentCommand>
{
    public BookAppointmentCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(x => x.ServiceId)
            .NotEmpty().WithMessage("Service ID is required.");

        RuleFor(x => x.StartTime)
            .GreaterThan(DateTime.UtcNow).WithMessage("Appointment time must be in the future.");
    }
}
