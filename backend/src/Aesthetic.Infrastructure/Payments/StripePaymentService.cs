using Aesthetic.Application.Common.Interfaces.Payments;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Registry;
using Stripe;

namespace Aesthetic.Infrastructure.Payments;

public class StripePaymentService : IPaymentService
{
    private readonly StripeSettings _stripeSettings;
    private readonly ResiliencePipeline _pipeline;

    public StripePaymentService(
        IOptions<StripeSettings> stripeSettings,
        ResiliencePipelineProvider<string> pipelineProvider)
    {
        _stripeSettings = stripeSettings.Value;
        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        _pipeline = pipelineProvider.GetPipeline("stripe-pipeline");
    }

    public async Task<string> CreatePaymentIntentAsync(decimal amount, string currency, string description, string? connectedAccountId = null, decimal applicationFeeAmount = 0, string? idempotencyKey = null, Dictionary<string, string>? metadata = null)
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(amount * 100), // Stripe expects amount in cents
            Currency = currency,
            Description = description,
            PaymentMethodTypes = new List<string> { "card" },
            Metadata = metadata
        };

        if (!string.IsNullOrEmpty(connectedAccountId))
        {
            options.TransferData = new PaymentIntentTransferDataOptions
            {
                Destination = connectedAccountId,
            };
            
            if (applicationFeeAmount > 0)
            {
                options.ApplicationFeeAmount = (long)(applicationFeeAmount * 100);
            }
        }

        var requestOptions = idempotencyKey != null ? new RequestOptions { IdempotencyKey = idempotencyKey } : null;

        var service = new PaymentIntentService();
        var paymentIntent = await _pipeline.ExecuteAsync(async ct => await service.CreateAsync(options, requestOptions, cancellationToken: ct));

        return paymentIntent.ClientSecret;
    }

    public async Task<string> CreateConnectedAccountAsync(string email)
    {
        var options = new AccountCreateOptions
        {
            Type = "express",
            Country = "US",
            Email = email,
            Capabilities = new AccountCapabilitiesOptions
            {
                CardPayments = new AccountCapabilitiesCardPaymentsOptions { Requested = true },
                Transfers = new AccountCapabilitiesTransfersOptions { Requested = true },
            },
        };
        var service = new AccountService();
        var account = await _pipeline.ExecuteAsync(async ct => await service.CreateAsync(options, cancellationToken: ct));
        return account.Id;
    }

    public async Task<string> CreateAccountLinkAsync(string accountId, string refreshUrl, string returnUrl)
    {
        var options = new AccountLinkCreateOptions
        {
            Account = accountId,
            RefreshUrl = refreshUrl,
            ReturnUrl = returnUrl,
            Type = "account_onboarding",
        };
        var service = new AccountLinkService();
        var link = await _pipeline.ExecuteAsync(async ct => await service.CreateAsync(options, cancellationToken: ct));
        return link.Url;
    }
}