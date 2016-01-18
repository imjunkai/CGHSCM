using CGHSCM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;


namespace CGHSCM.DAL
{
    public class Processes
    {
        private LogisticContext db = new LogisticContext(); 

        // Database seeding function
        public bool ProcessCSV(string name, string fileSavePath)
        {
            bool header = true;
            string[] data = System.IO.File.ReadAllLines(fileSavePath);
            char[] delimiter = { ',' };

            switch (name)
            {
                case "materials.csv":
                    db.Database.ExecuteSqlCommand("DELETE FROM Material");
                    foreach (string line in data)
                    {
                        if (header)
                        {
                            header = false;
                            continue;
                        }

                        string[] items = line.Split(delimiter);

                        Material m = new Material
                        {
                            MaterialID = items[0],
                            Description = items[1],
                            TopUp = int.Parse(items[2]),
                            UOM = items[3]
                        };

                        db.Materials.AddOrUpdate(m);
                    }
                    db.SaveChanges();
                    break;
                case "outstandings.csv":
                    db.Database.ExecuteSqlCommand("DELETE FROM Outstanding");
                    foreach (string line in data)
                    {
                        if (header)
                        {
                            header = false;
                            continue;
                        }

                        string[] items = line.Split(delimiter);
                        
                        DateTime TimeRequested = DateTime.Now;
                        DateTime? TimeDone = null, TimeUnlock = null;

                        bool isDone = int.Parse(items[2]) != 0 ? true : false;

                        DateTime.TryParse(items[3], out TimeRequested);
                        try { TimeDone = DateTime.Parse(items[4]); } catch { }
                        try { TimeUnlock = DateTime.Parse(items[5]); } catch { }

                        string materialID = items[0];
                        string costCenterName = items[1];

                        var material = (from m in db.Materials
                                        where m.MaterialID.ToUpper() == materialID.ToUpper()
                                        select new { m.Description, m.TopUp, m.UOM })
                                      .Single();

                        string costCenterID = (from c in db.CostCenters
                                               where c.CostCenterName.ToUpper().Contains(costCenterName.ToUpper())
                                               select new { c.CostCenterID })
                                               .Single()
                                               .CostCenterID;

                        Outstanding o = new Outstanding
                        {
                            MaterialID = materialID,
                            Description = material.Description,
                            TopUp = material.TopUp,
                            UOM = material.UOM,
                            CostCenterName = costCenterName,
                            CostCenterID = costCenterID,
                            IsDone = isDone,
                            TimeRequested = TimeRequested,
                            TimeDone = TimeDone,
                            TimeUnlock = TimeUnlock
                        };

                        db.Outstandings.AddOrUpdate(o);
                    }
                    db.SaveChanges();
                    break;
                case "costcenters.csv":
                    foreach (string line in data)
                    {
                        if (header)
                        {
                            header = false;
                            continue;
                        }

                        CostCenter cs = new CostCenter();

                        string[] items = line.Split(delimiter);
                        cs = new CostCenter { CostCenterID = items[0], CostCenterName = items[1] };
                        db.CostCenters.AddOrUpdate(cs);
                    }
                    db.SaveChanges();
                    break;
                default:
                    return false;
            }
            return true;
        }

        // Cron job that cleans up outstanding and moves them to past records
        public void UpdateOutstanding()
        {            

            var query = (from o in db.Outstandings
                         where o.IsDone == true && o.TimeUnlock <= DateTime.Now
                         select o).ToList();

            foreach (Outstanding o in query)
            {                
                db.Outstandings.Remove(o);                
            }
            db.SaveChanges();
        }

        // Changes outstanding items.isdone to true
        public void UpdateOutstanding(LogisticContext db, IEnumerable<Outstanding> outstandings)
        {
            foreach (Outstanding o in outstandings)
            {
                if (o.IsDone)
                {
                    MakeDoneItem(o);
                } else
                {
                    InsertItem(o);
                }
                                            
            }
        }

        private void InsertItem(Outstanding o)
        {
            try
            {
                var query = (from a in db.Outstandings
                             where
                                 a.CostCenterName == o.CostCenterName &&
                                 a.MaterialID == o.MaterialID &&
                                 (a.IsDone == false || a.TimeUnlock > DateTime.Now)
                             select a)
                             .Single();
            }
            catch
            {
                var material = (from m in db.Materials
                                where
                                    o.MaterialID == m.MaterialID
                                select
                                   new { m.Description, m.TopUp, m.UOM })
                            .Single();

                var costCenter = (from c in db.CostCenters
                                       where
                                            c.CostCenterName.ToUpper().Contains(o.CostCenterName.ToUpper())
                                       select
                                            new { c.CostCenterID })
                                        .Single();

                o.IsDone = false;
                o.CostCenterID = costCenter.CostCenterID;
                o.Description = material.Description;
                o.TopUp = material.TopUp;
                o.UOM = material.UOM;
                o.TimeRequested = DateTime.Now;

                db.Outstandings.Add(o);
                db.SaveChanges();
            }
        }

        private void MakeDoneItem(Outstanding o)
        {
            var result = db.Outstandings.SingleOrDefault(a => a.ID == o.ID);
            if (result != null)
            {
                result.TimeDone = DateTime.Now;
                result.TimeUnlock = DateTime.Now.AddHours(2);
                result.IsDone = true;                

                PastRecord p = new PastRecord
                {
                    CostCenterID = result.CostCenterID,
                    CostCenterName = result.CostCenterName,
                    Description = result.Description,
                    MaterialID = result.MaterialID,
                    TopUp = result.TopUp,
                    UOM = result.UOM,
                    TimeDone = (DateTime) result.TimeDone,
                    TimeRequested = result.TimeRequested
                };
                db.PastRecords.Add(p);

                db.SaveChanges();
            }            
        }

        public void DeleteOutstanding(Outstanding o)
        {
            var result = db.Outstandings.SingleOrDefault(a => a.ID == o.ID);
            if (result == null)
            {
                return;
            }
            db.Outstandings.Remove(result);
            db.SaveChanges();
        }
    }
}