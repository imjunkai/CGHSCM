using CGHSCM.Models;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace CGHSCM.DAL
{
    public class LogisticContext : DbContext
    {
        public LogisticContext() : base("LogisticDatabase")
        {

        }

        public DbSet<Material> Materials { get; set; }
        public DbSet<CostCenter> CostCenters { get; set; }
        public DbSet<Outstanding> Outstandings { get; set; }
        public DbSet<PastRecord> PastRecords { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}