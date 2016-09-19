using ContosoInsurance.Common.Data.CRM;

namespace ContosoInsurance.API.Models
{
    public class ApproveResult
    {
        public ClaimDamageAssessment DamageAssessment { get; set; }
        public bool IsPassed { get; set; }
    }
}