using CGHSCM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Migrations;


namespace CGHSCM.DAL
{
    public class TestSeeding : DropCreateDatabaseIfModelChanges<LogisticContext>
    {
        protected override void Seed(LogisticContext context)
        {
            var materials = new List<Material>
            {
                new Material {MaterialID = "AF101", Description = "iPhone 5", TopUp = 15, UOM = "PCS" },
                new Material {MaterialID = "PS331", Description = "Samsung S6", TopUp = 30, UOM = "PCS" },
                new Material {MaterialID = "NM449", Description = "Nokia Lumia 880", TopUp = 10, UOM = "PCS" }
            };

            materials.ForEach(m => context.Materials.Add(m));
            context.SaveChanges();
            
        }
    }
}