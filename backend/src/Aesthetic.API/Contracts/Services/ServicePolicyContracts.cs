namespace Aesthetic.API.Contracts.Services
{
    public record UpdateServicePoliciesRequest(
        decimal? DepositPercentage,
        decimal? CancelFeePercentage,
        int? CancelFeeWindowHours
    );
}
