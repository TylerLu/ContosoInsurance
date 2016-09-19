using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoInsurance.Common.Data.CRM
{
    public class ClaimImage
    {
        public int Id { get; set; }

        public int ClaimId { get; set; }

        [ForeignKey("ClaimId")]
        public virtual Claim Claim { get; set; }

        public string ImageUrl { get; set; }
    }
}
