using System;
using System.ComponentModel.DataAnnotations;


namespace CGHSCM.Models
{
    public class Outstanding
    {    
        public int ID { get; set; }

        [Required]
        public string MaterialID { get; set; }
        public string Description { get; set; }
        public int TopUp { get; set; }
        public string UOM { get; set; }

        public string CostCenterID { get; set; }
        [Required]
        public string CostCenterName { get; set; }
        [Required]
        public bool IsDone { get; set; }
        
        public DateTime TimeRequested { get; set; }
        public DateTime? TimeDone { get; set; }
        public DateTime? TimeUnlock { get; set; }


        public Outstanding()
        {
            IsDone = false;
            TimeRequested = DateTime.Now;
            if (TimeDone != null)
            {
                TimeUnlock = TimeDone.Value.AddHours(2);
            } 
        }            
    }
}