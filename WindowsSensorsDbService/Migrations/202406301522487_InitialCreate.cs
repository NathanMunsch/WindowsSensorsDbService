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
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.DateMeasurementEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ComputerEntityId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ComputerEntities", t => t.ComputerEntityId, cascadeDelete: true)
                .Index(t => t.ComputerEntityId);
            
            CreateTable(
                "dbo.MeasurementEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateMeasurementEntityId = c.Int(nullable: false),
                        SensorName = c.String(),
                        MeasuredValue = c.String(),
                        Unit = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DateMeasurementEntities", t => t.DateMeasurementEntityId, cascadeDelete: true)
                .Index(t => t.DateMeasurementEntityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DateMeasurementEntities", "ComputerEntityId", "dbo.ComputerEntities");
            DropForeignKey("dbo.MeasurementEntities", "DateMeasurementEntityId", "dbo.DateMeasurementEntities");
            DropIndex("dbo.MeasurementEntities", new[] { "DateMeasurementEntityId" });
            DropIndex("dbo.DateMeasurementEntities", new[] { "ComputerEntityId" });
            DropIndex("dbo.ComputerEntities", new[] { "Name" });
            DropTable("dbo.MeasurementEntities");
            DropTable("dbo.DateMeasurementEntities");
            DropTable("dbo.ComputerEntities");
        }
    }
}
