#r "Microsoft.WindowsAzure.Storage"

#load "..\Shared\CRM.csx"
#load "..\Shared\Mobile.csx"
#load "..\Shared\Utils.csx"
#load "..\Shared\Settings.csx"
#load "..\Shared\ApplicationInsights.csx"

#load "MobileClaim.csx"
#load "NewClaim.csx"
#load "OCR.csx"

using Microsoft.ApplicationInsights;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Data.Entity;

private static readonly string FunctionName = "HandleMobileClaim";

//Triggers on mobile-claims queue and writes to CRM Claims Database and new-claims queue, removes item from mobile-claims queue, invokes Logic App.
public static async Task Run(MobileClaim claim,
    ICloudBlob otherPartyCardImage, ICloudBlob otherPartyLicenseImage, ICloudBlob otherPartyPlateImages,
    ICloudBlob claimImage01, ICloudBlob claimImage02, ICloudBlob claimImage03, ICloudBlob claimImage04, ICloudBlob claimImage05,
    IAsyncCollector<NewClaim> newClaims)
{
    var telemetryClient = ApplicationInsights.CreateTelemetryClient();
    telemetryClient.TrackStatus(FunctionName, claim.Id, "Function triggered by mobile-claims queue");

    try
    {
        await HandleMobileClaim(claim, otherPartyCardImage, otherPartyLicenseImage, otherPartyPlateImages,
            claimImage01, claimImage02, claimImage03, claimImage04, claimImage05, newClaims, telemetryClient);
    }
    catch (Exception ex)
    {
        telemetryClient.TrackException(FunctionName, claim.Id, ex);
    }
    finally
    {
        telemetryClient.Flush();
    }
}

private static async Task HandleMobileClaim(MobileClaim claim,
    ICloudBlob otherPartyCardImage, ICloudBlob otherPartyLicenseImage, ICloudBlob otherPartyPlateImages,
    ICloudBlob claimImage01, ICloudBlob claimImage02, ICloudBlob claimImage03, ICloudBlob claimImage04, ICloudBlob claimImage05,
    IAsyncCollector<NewClaim> newClaims, TelemetryClient telemetryClient)
{
    var mobileClaim = await GetMobileClaimAsync(claim.Id);
    telemetryClient.TrackStatus(FunctionName, claim.Id, "Data queried from Mobile Claims SQL database", OperationStatus.Success);

    var CRMClaim = await GetCRMClaimAsync(mobileClaim,
        otherPartyCardImage, otherPartyLicenseImage, otherPartyPlateImages,
        new[] { claimImage01, claimImage02, claimImage03, claimImage04, claimImage05 },
        telemetryClient);

    // Write to CRM Claims Database 
    await AddCRMClaimAsync(CRMClaim);
    telemetryClient.TrackStatus(FunctionName, claim.Id, "Data inserted into CRM Claims SQL database", OperationStatus.Success);

    // Write to NewClaimForApprovalQueue 
    await newClaims.AddAsync(new NewClaim
    {
        Id = CRMClaim.Id,
        CorrelationId = claim.Id
    });
    telemetryClient.TrackStatus(FunctionName, claim.Id, "Data pushed into new-claims queue", OperationStatus.Success);
}

private static async Task<Mobile.Claim> GetMobileClaimAsync(string id)
{
    using (var db = new Mobile.ClaimsDbContext())
    {
        return await db.Claims
            .Where(i => i.Id == id)
            .FirstOrDefaultAsync();
    }
}

private static async Task<CRM.Claim> GetCRMClaimAsync(Mobile.Claim mobileClaim,
    ICloudBlob otherPartyCardImage, ICloudBlob otherPartyLicenseImage, ICloudBlob otherPartyPlateImages,
    IEnumerable<ICloudBlob> claimImages, TelemetryClient telemetryClient)
{
    var otherParty = new CRM.OtherParty
    {
        MobilePhone = mobileClaim.OtherPartyMobilePhone,
        InsuranceCardImageUrl = await Utils.GetBlobUriAsync(otherPartyCardImage),
        DriversLicenseImageUrl = await Utils.GetBlobUriAsync(otherPartyLicenseImage),
        LicensePlateImageUrl = await Utils.GetBlobUriAsync(otherPartyPlateImages)
    };
    if (otherPartyCardImage != null)
    {
        telemetryClient.TrackStatus(FunctionName, mobileClaim.Id, $"{CRM.ImageKind.InsuranceCard} OCR Started");
        await OCR.UpdateAsync(otherParty, CRM.ImageKind.InsuranceCard, await otherPartyCardImage.OpenReadAsync());
        telemetryClient.TrackStatus(FunctionName, mobileClaim.Id, $"{CRM.ImageKind.InsuranceCard} OCR Complete");
    }
    if (otherPartyLicenseImage != null)
    {
        telemetryClient.TrackStatus(FunctionName, mobileClaim.Id, $"{CRM.ImageKind.DriverLicense} OCR Started");
        await OCR.UpdateAsync(otherParty, CRM.ImageKind.DriverLicense, await otherPartyLicenseImage.OpenReadAsync());
        telemetryClient.TrackStatus(FunctionName, mobileClaim.Id, $"{CRM.ImageKind.DriverLicense} OCR Complete");
    }
    if (otherPartyPlateImages != null)
    {
        telemetryClient.TrackStatus(FunctionName, mobileClaim.Id, $"{CRM.ImageKind.LicensePlate} OCR Started");
        await OCR.UpdateAsync(otherParty, CRM.ImageKind.LicensePlate, await otherPartyPlateImages.OpenReadAsync());
        telemetryClient.TrackStatus(FunctionName, mobileClaim.Id, $"{CRM.ImageKind.LicensePlate} OCR Complete");
    }    
    if (!string.IsNullOrEmpty(otherParty.DriversLicenseNumber))
        otherParty.DriversLicenseNumber = otherParty.DriversLicenseNumber.Replace(" ", "");

    var CRMClaim = new CRM.Claim
    {
        Type = "Automobile",
        VehicleId = mobileClaim.VehicleId,
        OtherParty = otherParty,
        Coordinates = mobileClaim.Coordinates,
        DateTime = mobileClaim.DateTime,
        DueDate = mobileClaim.DateTime.AddDays(7),
        Status = CRM.ClaimStatus.Submitted,
        CorrelationId = Guid.Parse(mobileClaim.Id),
        Description = mobileClaim.Description,
    };
    foreach (var claimImage in claimImages)
    {
        if (!await claimImage.ExistsAsync()) continue;

        CRMClaim.Images.Add(new CRM.ClaimImage
        {
            Claim = CRMClaim,
            ImageUrl = claimImage.Uri.AbsoluteUri
        });
    }

    return CRMClaim;
}

private static async Task AddCRMClaimAsync(CRM.Claim claim)
{
    using (var db = new CRM.ClaimsDbContext())
    {
        db.Claims.Add(claim);
        await db.SaveChangesAsync();
    }
}