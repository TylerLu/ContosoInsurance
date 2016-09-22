#r "Newtonsoft.Json"

#load "..\Shared\CRM.csx"
#load "..\Shared\ApplicationInsights.csx"

using Microsoft.ApplicationInsights;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Net;

private static readonly string FunctionName = "AutoApproveClaim";

//Invoked by the HandleNewClaim Azure Function.  
//Queries the CRM Claims SQL database and looks to see if the current customer has submitted a claim before.  
//If no previous claims are found for the current customer, then auto approves the claim.  
//If previous claims are found for the current customer, then does not auto approve the claim.
public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    var jsonContent = await req.Content.ReadAsStringAsync();
    dynamic claim = JsonConvert.DeserializeObject(jsonContent);
    int? id = claim.id;
    string correlationId = claim.CorrelationId;

    if (id == null)
        return req.CreateResponse(HttpStatusCode.BadRequest, new { error = "Please pass id property in the input object." });

    var telemetryClient = ApplicationInsights.CreateTelemetryClient();
    try
    {
        var result = await ApproveClaimAsync(id.Value, correlationId, telemetryClient);
        return result ? "Approved" : "Rejected";
    }
    catch (Exception ex)
    {
        telemetryClient.TrackException(FunctionName, correlationId, ex);
        return req.CreateResponse(HttpStatusCode.InternalServerError, new { error = ex.Message });
    }
    finally
    {
        telemetryClient.Flush();
    }
}

private static async Task<bool> ApproveClaimAsync(int id, string correlationId, TelemetryClient telemetryClient)
{
    using (var db = new CRM.ClaimsDbContext())
    {
        var claim = await db.Claims
            .Include(i => i.Vehicle)
            .Where(i => i.Id == id)
            .FirstOrDefaultAsync();
        telemetryClient.TrackStatus(FunctionName, correlationId, "Data queried from CRM Claims SQL database", true);
        if (claim == null) throw new ApplicationException("Could not found the claim");

        var isTheFirstTime = !await db.Claims
            .Where(i => i.Id != claim.Id)
            .Where(i => i.Vehicle.CustomerId == claim.Vehicle.CustomerId)
            .Where(i => i.DateTime < claim.DateTime)
            .AnyAsync();

        var approved = isTheFirstTime;
        claim.Status = approved ? CRM.ClaimStatus.AutoApproved : CRM.ClaimStatus.AutoRejected;

        await db.SaveChangesAsync();
        telemetryClient.TrackStatus(FunctionName, correlationId, "Data updated in CRM Claims SQL database", true);

        return approved;
    }
}