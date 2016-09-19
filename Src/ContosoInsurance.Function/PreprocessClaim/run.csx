#r "Newtonsoft.Json"

#load "..\Shared\CRM.csx"
#load "..\Shared\ApplicationInsights.csx"

#load "OCR.csx"

using Microsoft.ApplicationInsights;
using Newtonsoft.Json;
using System.Data.Entity;
using System.IO;
using System.Net;

private static readonly string FunctionName = "PreprocessClaim";

//Invoked by HandleMobileClaim Azure Function.  Returns current claim and otherparty from CRM Claims SQL database.  
//Calls OCR to process license plate, driver’s license, and insurance card other party images. 
//Updates the current claim and other party in CRM Claims SQL database with the data returned from the OCR process.
public static async Task<HttpResponseMessage> Run(HttpRequestMessage req,
    Stream otherPartyLicensePlateImageStream, Stream otherPartyInsuranceCardImageStream,
    Stream otherPartyDriverLicenseImageStream, TraceWriter log)
{
    var jsonContent = await req.Content.ReadAsStringAsync();
    dynamic claim = JsonConvert.DeserializeObject(jsonContent);
    int? id = claim.id;
    string correlationId = claim.correlationId;

    if (id == null)
        return req.CreateResponse(HttpStatusCode.BadRequest, new { error = "Please pass id property in the input object." });
    
    var telemetryClient = ApplicationInsights.CreateTelemetryClient();

    try
    {
        await PreprocessClaimAsync(
            id.Value,
            correlationId,
            otherPartyLicensePlateImageStream,
            otherPartyInsuranceCardImageStream,
            otherPartyDriverLicenseImageStream,
            telemetryClient);
        return req.CreateResponse(HttpStatusCode.OK);
    }
    catch (Exception ex)
    {
        telemetryClient.TraceException(FunctionName, correlationId, ex);
        return req.CreateResponse(HttpStatusCode.InternalServerError, new { error = ex.Message });
    }
    finally
    {
        telemetryClient.Flush();
    }
}

private static async Task PreprocessClaimAsync(int id, string correlationId, Stream otherPartyLicensePlateImageStream,
    Stream otherPartyInsuranceCardImageStream, Stream otherPartyDriverLicenseImageStream, TelemetryClient telemetryClient)
{
    using (var db = new CRM.ClaimsDbContext())
    {
        var claim = await db.Claims
            .Include(i => i.OtherParty)
            .Where(i => i.Id == id)
            .FirstOrDefaultAsync();
        telemetryClient.TraceStatus(FunctionName, correlationId, "Data queried from CRM Claims SQL database", true);
        if (claim == null) throw new ApplicationException("Could not find the claim");

        var otherParty = claim.OtherParty;
                
        if (otherPartyLicensePlateImageStream != null)
        {
            telemetryClient.TraceStatus(FunctionName, correlationId, $"{CRM.ImageKind.LicensePlate} OCR Started");
            await OCR.UpdateAsync(otherParty, CRM.ImageKind.LicensePlate, otherPartyLicensePlateImageStream);
            telemetryClient.TraceStatus(FunctionName, correlationId, $"{CRM.ImageKind.LicensePlate} OCR Complete");
        }
        if (otherPartyInsuranceCardImageStream != null)
        {
            telemetryClient.TraceStatus(FunctionName, correlationId, $"{CRM.ImageKind.InsuranceCard} OCR Started");
            await OCR.UpdateAsync(otherParty, CRM.ImageKind.InsuranceCard, otherPartyInsuranceCardImageStream);
            telemetryClient.TraceStatus(FunctionName, correlationId, $"{CRM.ImageKind.InsuranceCard} OCR Complete");
        }
        if (otherPartyDriverLicenseImageStream != null)
        {
            telemetryClient.TraceStatus(FunctionName, correlationId, $"{CRM.ImageKind.DriverLicense} OCR Started");
            await OCR.UpdateAsync(otherParty, CRM.ImageKind.DriverLicense, otherPartyDriverLicenseImageStream);
            telemetryClient.TraceStatus(FunctionName, correlationId, $"{CRM.ImageKind.DriverLicense} OCR Complete");
        }

        if (!string.IsNullOrEmpty(otherParty.DriversLicenseNumber))
            otherParty.DriversLicenseNumber = otherParty.DriversLicenseNumber.Replace(" ", "");

        await db.SaveChangesAsync();
        telemetryClient.TraceStatus(FunctionName, correlationId, "Data updated in CRM Claims SQL database", true);
    }
}