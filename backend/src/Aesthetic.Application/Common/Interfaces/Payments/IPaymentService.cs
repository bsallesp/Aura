namespace Aesthetic.Application.Common.Interfaces.Payments;

public interface IPaymentService
{
    Task<string> CreatePaymentIntentAsync(decimal amount, string currency, string description, string? connectedAccountId = null, decimal applicationFeeAmount = 0, string? idempotencyKey = null, Dictionary<string, string>? metadata = null);
    Task<string> CreateConnectedAccountAsync(string email);
    Task<string> CreateAccountLinkAsync(string accountId, string refreshUrl, string returnUrl);
}