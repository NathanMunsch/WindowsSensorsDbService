using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsSensorsDbService.Helpers;
using WindowsSensorsDbService.Migrations;
using WindowsSensorsDbService.Models;

namespace WindowsSensorsDbService.Data
{
    public class DataContext : DbContext
    {
        public DbSet<ComputerEntity> ComputerEntities { get; set; }
        public DbSet<DateMeasurementEntity> DateMeasurementEntities { get; set; }
        public DbSet<MeasurementEntity> MeasurementEntities { get; set; }
        public DbSet<HardwareEntity> HardwareEntities { get; set; }

        public DataContext() : base(ConnectionString.GetConnectionString())
        {
            this.Configuration.LazyLoadingEnabled = true;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ComputerEntity>()
                .HasMany(c => c.DateMeasurementEntities)
                .WithRequired(dm => dm.ComputerEntity)
                .HasForeignKey(dm => dm.ComputerEntityId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DateMeasurementEntity>()
                .HasMany(dm => dm.MeasurementEntities)
                .WithRequired(m => m.DateMeasurementEntity)
                .HasForeignKey(m => m.DateMeasurementEntityId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HardwareEntity>()
                .HasMany(h => h.MeasurementEntities)
                .WithRequired(m => m.HardwareEntity)
                .HasForeignKey(m => m.HardwareEntityId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
