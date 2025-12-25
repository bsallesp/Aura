namespace Aesthetic.API.Contracts.Payments
{
    public record CreatePaymentIntentRequest(
        decimal Amount,
        string Currency,
        string Description
    );

    public record CreatePaymentIntentResponse(
        string ClientSecret
    );
}