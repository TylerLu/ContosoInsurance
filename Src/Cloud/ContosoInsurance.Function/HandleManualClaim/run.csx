#r "Microsoft.WindowsAzure.Storage"

#load "..\Shared\CRM.csx"
#load "..\Shared\ApplicationInsights.csx"

#load "Claim.csx"

using Microsoft.ApplicationInsights;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Net;

private static readonly string FunctionName = "HandleManualClaim";

// Invoked by the ContosoClaimManualApprover Logic app to update the CRM Claims Database.
public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    var telemetryClient = ApplicationInsights.CreateTelemetryClient();

    var jsonContent = await req.Content.ReadAsStringAsync();
    var claim = JsonConvert.DeserializeObject<Claim>(jsonContent);

    try
    {
        await ApproveClaimAsync(claim.Id, claim.CorrelationId, claim.DamageAssessment, claim.Approved, telemetryClient);
        return req.CreateResponse(HttpStatusCode.OK);
    }
    catch (Exception ex)
    {
        telemetryClient.TrackException(FunctionName, claim.CorrelationId, ex);
        return req.CreateResponse(HttpStatusCode.BadRequest, new { error = ex.Message });
    }
    finally
    {
        telemetryClient.Flush();
    }
}

private static async Task ApproveClaimAsync(int id, string correlationId, CRM.ClaimDamageAssessment damageAssessment, bool approved, TelemetryClient telemetryClient)
{
    using (var db = new CRM.ClaimsDbContext())
    {
        var claim = await db.Claims
            .Include(i => i.Vehicle)
            .Where(i => i.Id == id)
            .FirstOrDefaultAsync();
        telemetryClient.TrackStatus(FunctionName, correlationId, "Data queried from CRM Claims SQL database", true);
        if (claim == null) throw new ApplicationException("Could not find the claim");

        claim.DamageAssessment = damageAssessment;
        claim.Status = approved ? CRM.ClaimStatus.ManualApproved : CRM.ClaimStatus.ManualRejected;
        await db.SaveChangesAsync();
        telemetryClient.TrackStatus(FunctionName, correlationId, "Data updated in CRM Claims SQL database", true);

        var description = approved ? "Claim Manually Approved" : "Claim Manually Rejected";
        telemetryClient.TrackStatus(FunctionName, correlationId, description, true);
    }
}