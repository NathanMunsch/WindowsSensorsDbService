namespace WindowsSensorsDbService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComputerEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DateMeasurementEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ComputerEntityId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ComputerEntities", t => t.ComputerEntityId)
                .Index(t => t.ComputerEntityId);
            
            CreateTable(
                "dbo.MeasurementEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateMeasurementEntityId = c.Int(nullable: false),
                        HardwareEntityId = c.Int(nullable: false),
                        SensorName = c.String(),
                        MeasuredValue = c.String(),
                        Unit = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HardwareEntities", t => t.HardwareEntityId)
                .ForeignKey("dbo.DateMeasurementEntities", t => t.DateMeasurementEntityId)
                .Index(t => t.DateMeasurementEntityId)
                .Index(t => t.HardwareEntityId);
            
            CreateTable(
                "dbo.HardwareEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DateMeasurementEntities", "ComputerEntityId", "dbo.ComputerEntities");
            DropForeignKey("dbo.MeasurementEntities", "DateMeasurementEntityId", "dbo.DateMeasurementEntities");
            DropForeignKey("dbo.MeasurementEntities", "HardwareEntityId", "dbo.HardwareEntities");
            DropIndex("dbo.MeasurementEntities", new[] { "HardwareEntityId" });
            DropIndex("dbo.MeasurementEntities", new[] { "DateMeasurementEntityId" });
            DropIndex("dbo.DateMeasurementEntities", new[] { "ComputerEntityId" });
            DropTable("dbo.HardwareEntities");
            DropTable("dbo.MeasurementEntities");
            DropTable("dbo.DateMeasurementEntities");
            DropTable("dbo.ComputerEntities");
        }
    }
}
