using System.ComponentModel.DataAnnotations;


namespace CGHSCM.Models
{
    public class Material
    {
        public Material()
        {
            TopUp = 10;
            UOM = "PCS";
        }

        [Required]        
        public string MaterialID { get; set; }
        [Required]        
        public string Description { get; set; }
        [Required]        
        public int TopUp { get; set; }
        [Required]
        public string UOM { get; set; }
    }
}