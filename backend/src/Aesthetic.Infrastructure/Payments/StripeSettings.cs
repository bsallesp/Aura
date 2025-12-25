namespace Aesthetic.Infrastructure.Payments;

public class StripeSettings
{
    public const string SectionName = "StripeSettings";
    public string SecretKey { get; init; } = null!;
    public string PublishableKey { get; init; } = null!;
}