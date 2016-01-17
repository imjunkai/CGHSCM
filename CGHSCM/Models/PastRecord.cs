using System;

namespace CGHSCM.Models
{
    public class PastRecord
    {
        public int ID { get; set; }

        public string MaterialID { get; set; }
        public string Description { get; set; }
        public int TopUp { get; set; }
        public string UOM { get; set; }

        public string CostCenterID { get; set; }
        public string CostCenterName { get; set; }        

        public DateTime TimeRequested { get; set; }
        public DateTime TimeDone { get; set; }
    }
}