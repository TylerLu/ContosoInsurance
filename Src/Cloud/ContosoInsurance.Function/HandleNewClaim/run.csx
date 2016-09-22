#r "Microsoft.WindowsAzure.Storage"

#load "..\Shared\CRM.csx"
#load "..\Shared\Utils.csx"
#load "..\Shared\Settings.csx"
#load "..\Shared\ApplicationInsights.csx"

#load "Claim.csx"

using Microsoft.ApplicationInsights;
using System.Data.Entity;

private static readonly string FunctionName = "HandleNewClaim";

//Triggers on new- claims queue and invokes Logic App, removes items from new- claims queue.
public static async Task Run(Claim newClaim, TraceWriter log)
{
    var telemetryClient = ApplicationInsights.CreateTelemetryClient();
    telemetryClient.TrackStatus(FunctionName, newClaim.CorrelationId, "Function triggered by new-claims queue");

    try
    {
        await HandleNewClaim(newClaim, telemetryClient);
    }
    catch (Exception ex)
    {
        telemetryClient.TrackException(FunctionName, newClaim.CorrelationId, ex);
    }
    finally
    {
        telemetryClient.Flush();
    }
}

private static async Task HandleNewClaim(Claim claim, TelemetryClient telemetryClient)
{
    var customer = await GetCustomer(claim.Id);
    telemetryClient.TrackStatus(FunctionName, claim.CorrelationId, "Data queried from CRM Claims SQL database", true);
    if (customer == null) return;

    var payload = new
    {
        id = claim.Id,
        correlationId = claim.CorrelationId,
        customerName = customer.FirstName + " " + customer.LastName,
        customerEmail = customer.Email,
        claimsAdjusterEmail = Settings.ClaimsAdjusterEmail,
        claimDetailsPageBaseUrl = Settings.ClaimDetailsPageBaseUrl
    };
    var response = await Utils.PostTo(Settings.ClaimAutoApproverUrl, payload);
    telemetryClient.TrackStatus(FunctionName, claim.CorrelationId, "Invoked ClaimAutoApprover Azure Function", response.IsSuccessStatusCode);
}

private static async Task<CRM.Customer> GetCustomer(int claimId)
{
    using (var db = new CRM.ClaimsDbContext())
    {
        return await db.Claims
            .Where(i => i.Id == claimId)
            .Select(i => i.Vehicle.Customer)
            .FirstOrDefaultAsync();
    }
}