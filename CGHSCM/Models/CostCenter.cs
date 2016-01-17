using System.ComponentModel.DataAnnotations;


namespace CGHSCM.Models
{
    public class CostCenter
    {
        [Required]
        public string CostCenterID { get; set; }        
        [Required]
        public string CostCenterName { get; set; }
    }
}