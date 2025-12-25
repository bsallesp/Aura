namespace Aesthetic.API.Contracts.Payments
{
    public record CreatePaymentIntentRequest(
        Guid AppointmentId
    );

    public record CreatePaymentIntentResponse(
        string ClientSecret
    );
}