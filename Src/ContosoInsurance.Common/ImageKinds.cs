namespace ContosoInsurance.Common
{
    public static class ImageKinds
    {
        public static readonly string Incident = "claim";
        public static readonly string Vehicle = "vehicle";
        
        public static class OtherParty
        {
            public static readonly string LicensePlate = "other-party-plate";
            public static readonly string InsuranceCard = "other-party-card";
            public static readonly string DriverLicense = "other-party-license";
        }

        public static readonly string[] AllImageKinds =
        {
            Vehicle,
            Incident,
            OtherParty.LicensePlate,
            OtherParty.InsuranceCard,
            OtherParty.DriverLicense
        };

        public static readonly string[] AllClaimImageKinds =
        {
            Incident,
            OtherParty.LicensePlate,
            OtherParty.InsuranceCard,
            OtherParty.DriverLicense
        };
    }
}