public class Claim
{
    public int Id { get; set; }
    public string CorrelationId { get; set; }
    public CRM.ClaimDamageAssessment DamageAssessment { get; set; }
    public bool Approved { get; set; }
}