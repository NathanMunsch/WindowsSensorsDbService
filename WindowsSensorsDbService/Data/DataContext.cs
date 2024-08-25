using Microsoft.EntityFrameworkCore;
using WindowsSensorsDbService.Models;

namespace WindowsSensorsDbService.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<ComputerEntity> ComputerEntities { get; set; }
    public DbSet<DateMeasurementEntity> DateMeasurementEntities { get; set; }
    public DbSet<MeasurementEntity> MeasurementEntities { get; set; }
    public DbSet<HardwareEntity> HardwareEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ComputerEntity>()
            .HasMany(c => c.DateMeasurementEntities)
            .WithOne(dm => dm.ComputerEntity)
            .HasForeignKey(dm => dm.ComputerEntityId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DateMeasurementEntity>()
            .HasMany(dm => dm.MeasurementEntities)
            .WithOne(m => m.DateMeasurementEntity)
            .HasForeignKey(m => m.DateMeasurementEntityId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<HardwareEntity>()
            .HasMany(h => h.MeasurementEntities)
            .WithOne(m => m.HardwareEntity)
            .HasForeignKey(m => m.HardwareEntityId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}